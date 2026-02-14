using Steffi.Models;
using Steffi.Models.Builder;
using Steffi.Models.Interfaces;
using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public class SteffiParser
{
	private int _noNameCount = 0;
	private SteffiDocument _steffiDocument = new SteffiDocument();
	private Stack<SteffiObject> _parents = new();

	public async Task<(SteffiDocument?, List<string> Errors)> ParseFromFileAsync(string fileName)
	{
		string content = await File.ReadAllTextAsync(fileName);

		return Parse(content);
	}

	protected void Initialize()
	{
		_steffiDocument = new SteffiDocument();
		_parents = new([_steffiDocument]);
		_noNameCount = 0;
	}

	protected SteffiDocument Document { get => _steffiDocument; }

	protected SteffiObject CurrentObject { get => _parents.Peek(); }

	protected bool NestObject(SteffiObject steffiObject)
	{
		var currentParent = _parents.Peek();

		if (currentParent is not IParentObject parent)
		{
			return false;
		}

		parent.Children.Add(steffiObject);
		_parents.Push(steffiObject);

		return true;
	}

	public (SteffiDocument?, List<string> Errors) Parse(string content)
	{
		Initialize();
		var parsingContext = new ParsingContext(content.AsSpan());

		while (!parsingContext.IsEndReached)
		{
			if (parsingContext.Matches(Syntax.ObjectDeclaration, out var typeIdentifierSegment, out var objectNameSegment))
			{
				var objectName = objectNameSegment.GetValueOrDefault(GenerateDefaultName);
				var typeName = typeIdentifierSegment.Value;

				if (CurrentObject is IParentObject parent)
				{
					var steffiObject = ModelBuilder.CreateObjectFactory(typeName, objectName, parent);
					if (steffiObject == null)
					{
						return (Document, [typeIdentifierSegment.CreateError($"Unknown type '{typeName}'")]);
					}

					if (!NestObject(steffiObject))
					{
						return (Document, [$"{typeIdentifierSegment.GetPositionString()} Cannot nest children in {CurrentObject.GetType().Name}"]);
					}
				}
				else
				{
					return (Document, [typeIdentifierSegment.CreateError($"Cannot nest children in {CurrentObject.GetType().Name}")]);
				}

				continue;
			}

			if (parsingContext.Matches(Syntax.PropertyAssignment, out var propertyNameSegment, out var propertyValueSegment))
			{
				var propertyName = propertyNameSegment.Value;
				var propertyValue = propertyValueSegment.Value;

				if (propertyValue.Matches(Terms.StringLiteral))
				{
					propertyValue = propertyValue[1..^1].ToString();
				}

				if (!ModelBuilder.SetObjectProperty(CurrentObject, propertyName, propertyValue.Trim()))
				{
					return (Document, [typeIdentifierSegment.CreateError($"Cannot assign property '{propertyName}' to value {propertyValue}")]);
				}

				continue;
			}

			if (parsingContext.Matches(Terms.NestingClose.ExactlyOne()))
			{
				_parents.Pop();
				continue;
			}

			if (parsingContext.Matches((Terms.WhiteSpaceBlock | Terms.BlockComment | Terms.LineComment).OneOrMore()))
			{
				continue;
			}

			return (Document, [parsingContext.CreateError($"Unexpected expression")]);
		}

		if (_parents.Count == 1)
		{
			Document.ResolveReferences();
		}

		return _parents.Count switch
		{
			> 1 => (Document, [parsingContext.CreateError("Unexpected end of file, object not closed")]),
			0 => (Document, [parsingContext.CreateError("Unexpected end of file, object not closed")]),
			1 => (Document, []),
			_ => throw new InvalidOperationException($"Unreachable code reached in {nameof(SteffiParser)}"),
		};
	}

	private ReadOnlySpan<char> GenerateDefaultName() => $"NoNameObject{_noNameCount++}";
}
