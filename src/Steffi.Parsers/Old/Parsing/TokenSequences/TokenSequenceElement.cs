using Steffi.Parsers.Old.Parsing.Lexer;

namespace Steffi.Parsers.Old.Parsing.TokenSequences;

public abstract class TokenSequenceElement
{
	public abstract bool IsMatch(ParsedToken parsedToken);
}
