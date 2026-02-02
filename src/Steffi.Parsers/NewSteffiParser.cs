using Steffi.Parsers.Model;
using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public class SteffiParser
{
	private int _noNameCount = 0;

	public async Task<(SteffiDocument?, List<string> Errors)> ParseFromFileAsync(string fileName)
	{
		string content = await File.ReadAllTextAsync(fileName);

		return Parse(content);
	}

	public (SteffiDocument?, List<string> Errors) Parse(string content)
	{
		var parsingContext = new ParsingContext(content.AsSpan());
		Stack<SteffiObject> parentList = new([new SteffiDocument()]);

		while (!parsingContext.EndReached)
		{
			if (parsingContext.Match(Syntax.ObjectDeclaration, out var objectDeclarationSegments))
			{
				var objectName = objectDeclarationSegments
					.SingleOrDefault(s => s.Name == "ObjectName")
					.GetValue(parsingContext);

				if (objectName.Length == 0)
				{
					objectName = $"NoNameObject{_noNameCount++}";
				}

				var typeNameSegment = objectDeclarationSegments.Single(s => s.Name == "TypeIdentifier");
				var typeName = typeNameSegment.GetValue(parsingContext);

				var steffiObject = SteffiObjectBuilder.CreateObjectFactory(typeName, objectName);

				if (steffiObject == null)
				{
					return (null, [$"{typeNameSegment.GetPositionString()} Unknown type '{typeName}'"]);
				}

				var currentParent = parentList.Peek();
				if (currentParent is IParentObject parent)
				{
					parent.Children.Add(steffiObject);
					parentList.Push(steffiObject);
				}
				else
				{
					return (null, [$"{typeNameSegment.GetPositionString()} Cannot nest children at {currentParent.GetType()}"]);
				}

				continue;
			}

			if (parsingContext.Match(Syntax.PropertyAssignment, out var propertyAssignmentSegments))
			{
				var propertyNameSegment = propertyAssignmentSegments.Single(s => s.Name == "PropertyName");
				var propertyValueSegment = propertyAssignmentSegments.Single(s => s.Name == "PropertyValue");

				var propertyName = propertyNameSegment.GetValue(parsingContext);
				var propertyValue = propertyValueSegment.GetValue(parsingContext);

				var currentParent = parentList.Peek();
				SteffiObjectBuilder.SetObjectProperty(currentParent, propertyName, propertyValue.Trim());

				continue;
			}

			if (parsingContext.Match(Terms.NestingClose.ExactlyOne(), out var _))
			{
				parentList.Pop();
				continue;
			}

			if (parsingContext.Match((Terms.WhiteSpace | Terms.BlockComment | Terms.LineComment).OneOrMore(), out var _))
			{
				continue;
			}

			return (null, [$"{parsingContext.GetPositionString()} Unexpected token '{parsingContext.Remaining.Slice(0, Math.Max(10, parsingContext.Remaining.Length))}'"]);
		}

		return parentList.Count switch
		{
			> 1 => (null, [$"Unexpected end of file, object not closed"]),
			0 => (null, [$"Unexpected end of file, object not closed"]),
			1 => ((SteffiDocument)parentList.Pop(), []),
			_ => throw new InvalidOperationException($"Unreachable code reached in {nameof(SteffiParser)}"),
		};
	}
}
