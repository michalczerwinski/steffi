using Steffi.Parsers.Parsing;

namespace Steffi.Parsers.Parsing.TokenSequences;

public abstract class TokenSequenceElement
{
	public abstract bool IsMatch(ParsedToken parsedToken);
}
