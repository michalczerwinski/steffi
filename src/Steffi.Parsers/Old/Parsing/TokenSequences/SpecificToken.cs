using Steffi.Parsers.Old.Parsing;
using Steffi.Parsers.Old.Parsing.Lexer;

namespace Steffi.Parsers.Old.Parsing.TokenSequences;

public class SpecificToken(Token token) : TokenSequenceElement
{
	public override bool IsMatch(ParsedToken parsedToken) => parsedToken.TokenParser == token;
}
