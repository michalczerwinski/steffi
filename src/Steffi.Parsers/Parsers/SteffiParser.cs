using Steffi.Parsers.Model;
using Steffi.Parsers.Parsing;
using Steffi.Parsers.Parsing.TokenSequences;

namespace Steffi.Parsers.Parsers;

public class SteffiParser : ParserBase<SteffiDocument>
{
	private static TokenSequence ObjectDeclaration = new TokenSequence()
		.AddSegment("Trivia1", new OneOfManyTokens([SteffiLexer.LineComment, SteffiLexer.WhiteSpace]), SegmentType.OptionalMultiple)
		.AddSegment("TypeIdentifier", new SpecificToken(SteffiLexer.Identifier), SegmentType.RequiredOnce)
		.AddSegment("OptionalName", new SpecificToken(SteffiLexer.Identifier), SegmentType.OptionalOnce)
		.AddSegment("Trivia2", new OneOfManyTokens([SteffiLexer.LineComment, SteffiLexer.WhiteSpace]), SegmentType.OptionalMultiple)
		.AddSegment("NestingOpen", new SpecificToken(SteffiLexer.NestingOpen), SegmentType.RequiredOnce);

	private SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, IDictionary<string, object> properties)
	{
		return tokenType switch
		{
			"Node" => new Node { Name = (string)properties[nameof(Node.Name)] },
			"Graph" => new Graph { Name = (string)properties[nameof(Graph.Name)] },
			_ => null,
		};
	}

	protected override (SteffiDocument?, List<string> Errors) GenerateSyntaxTree(ParsingContext parsingContext)
	{
		parsingContext
			.Tokens
			.RemoveAll(t => t.TokenParser == SteffiLexer.WhiteSpace || t.TokenParser == SteffiLexer.LineComment);

		Stack<SteffiObject> parentList = new([new SteffiDocument()]);
		int noNameTokens = 0;

		while (parsingContext.Tokens.Any())
		{
			if (ObjectDeclaration.Match(parsingContext, out var matchedSegments) is int matchLength && matchLength != -1)
			{
				parsingContext.MoveAheadTokens(matchLength);
				var optionalNameTokens = matchedSegments["OptionalName"];
				string objectName = optionalNameTokens.Any()
					? parsingContext.GetTokenValue(matchedSegments["OptionalName"].Single()).ToString()
					: $"noName{++noNameTokens}";

				var typeToken = matchedSegments["TypeIdentifier"].Single();
				var type = parsingContext.GetTokenValue(typeToken);

				var steffiObject = CreateObjectFactory(type, new Dictionary<string, object> { { "Name", objectName } });

				if (steffiObject == null)
				{
					return (null, [$"{typeToken.GetPositionString()} Unknown type '{type}'"]);
				}

				var currentParent = parentList.Peek();
				if (currentParent is IParentObject parent)
				{
					parent.Children.Add(steffiObject);
				}
				else
				{
					return (null, [$"{typeToken.GetPositionString()} Cannot nest children at {currentParent.GetType()}"]);
				}

				var lastObject = ((IParentObject)parentList.Peek()).Children.Last();
				parentList.Push(lastObject);
				continue;
			}

			if (parsingContext.PeekNextToken().TokenParser == SteffiLexer.NestingClose)
			{
				parsingContext.MoveAheadTokens(1);
				parentList.Pop();
				continue;
			}

			var unexpectedToken = parsingContext.PeekNextToken();
			return (null, [$"{unexpectedToken.GetPositionString()} Unexpected token '{parsingContext.GetTokenValue(unexpectedToken)}'"]);
		}

		var steffiDocument = (SteffiDocument)parentList.Pop();

		return (steffiDocument, []);
	}
}
