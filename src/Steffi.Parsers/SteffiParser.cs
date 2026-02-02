using Steffi.Parsers.Model;
using Steffi.Parsers.Model.Builder;
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
		var document = new SteffiDocument();
		Stack<SteffiObject> parents = new([document]);

		while (!parsingContext.EndReached)
		{
			if (parsingContext.Matches(Syntax.ObjectDeclaration, out var typeIdentifierSegment, out var objectNameSegment))
			{
				var objectName = objectNameSegment.GetValueOrDefault(parsingContext, () => $"NoNameObject{_noNameCount++}");
				var typeName = typeIdentifierSegment.GetValue(parsingContext);

				var steffiObject = ModelBuilder.CreateObjectFactory(typeName, objectName);

				if (steffiObject == null)
				{
					return (document, [typeIdentifierSegment.CreateError($"Unknown type '{typeName}'")]);
				}

				var currentParent = parents.Peek();
				if (currentParent is IParentObject parent)
				{
					parent.Children.Add(steffiObject);
					parents.Push(steffiObject);
				}
				else
				{
					return (document, [$"{typeIdentifierSegment.GetPositionString()} Cannot nest children at {currentParent.GetType()}"]);
				}

				continue;
			}

			if (parsingContext.Matches(Syntax.PropertyAssignment, out var propertyNameSegment, out var propertyValueSegment))
			{
				var propertyName = propertyNameSegment.GetValue(parsingContext);
				var propertyValue = propertyValueSegment.GetValue(parsingContext);

				var currentParent = parents.Peek();
				ModelBuilder.SetObjectProperty(currentParent, propertyName, propertyValue.Trim());

				continue;
			}

			if (parsingContext.Matches(Terms.NestingClose.ExactlyOne()))
			{
				parents.Pop();
				continue;
			}

			if (parsingContext.Matches((Terms.WhiteSpaceBlock | Terms.BlockComment | Terms.LineComment).OneOrMore()))
			{
				continue;
			}

			return (null, [parsingContext.CreateError($"Unexpected expression")]);
		}

		return parents.Count switch
		{
			> 1 => (document, [parsingContext.CreateError("Unexpected end of file, object not closed")]),
			0 => (document, [parsingContext.CreateError("Unexpected end of file, object not closed")]),
			1 => (document, []),
			_ => throw new InvalidOperationException($"Unreachable code reached in {nameof(SteffiParser)}"),
		};
	}
}
