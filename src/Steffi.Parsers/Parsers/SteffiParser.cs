using Steffi.Parsers.Model;
using Steffi.Parsers.Parsing;
using Steffi.Parsers.Parsing.TokenSequences;

namespace Steffi.Parsers.Parsers;

public class SteffiParser : ParserBase<SteffiDocument>
{
	private static readonly TokenSequence ObjectDeclaration = new TokenSequence()
		.AddSegment("TypeIdentifier", new SpecificToken(SteffiLexer.Identifier), Arity.RequiredOnce)
		.AddSegment("OptionalName", new SpecificToken(SteffiLexer.Identifier), Arity.OptionalOnce)
		.AddSegment("NestingOpen", new SpecificToken(SteffiLexer.NestingOpen), Arity.RequiredOnce);

	private static SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, string name)
	{
		return tokenType switch
		{
			"Node" => new Node { Name = name },
			"Graph" => new Graph { Name = name },
			_ => null,
		};
	}

	protected override (SteffiDocument?, List<string> Errors) GenerateSyntaxTree(ParsingContext parsingContext)
	{
		Stack<SteffiObject> parentList = new([new SteffiDocument()]);
		int noNameTokens = 0;

		while (parsingContext.Tokens.Finished())
		{
			if (ObjectDeclaration.Match(parsingContext, out var matchedSegments) is int matchLength && matchLength != -1)
			{
				parsingContext.Tokens.Move(matchLength);
				var optionalNameTokens = matchedSegments["OptionalName"];
				string objectName = optionalNameTokens.Count != 0
					? parsingContext.GetTokenValue(matchedSegments["OptionalName"].Single()).ToString()
					: $"noName{++noNameTokens}";

				var typeToken = matchedSegments["TypeIdentifier"].Single();
				var type = parsingContext.GetTokenValue(typeToken);

				var steffiObject = CreateObjectFactory(type, objectName);

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

			if (parsingContext.Tokens.PeekNext(out var nextToken))
			{
				if (nextToken!.TokenParser == SteffiLexer.NestingClose)
				{
					parsingContext.Tokens.Move(1);
					parentList.Pop();
					continue;
				}

				return (null, [$"{nextToken.GetPositionString()} Unexpected token '{parsingContext.GetTokenValue(nextToken)}'"]);
			}
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
