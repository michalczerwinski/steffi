using Steffi.Parsers.Old.Parsing;
using Steffi.Parsers.Old.Parsing.Lexer;

namespace Steffi.Parsers.Old.Parsing.TokenSequences;

public class OneOfManyTokens(params Token[] tokens) : TokenSequenceElement
{
	public override bool IsMatch(ParsedToken parsedToken) => tokens.Any(t => t == parsedToken.TokenParser);
}

