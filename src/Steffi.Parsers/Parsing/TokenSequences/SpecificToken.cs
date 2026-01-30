namespace Steffi.Parsers.Parsing.TokenSequences;

public class SpecificToken(Token token) : TokenSequenceElement
{
	public override bool IsMatch(ParsedToken parsedToken) => parsedToken.TokenParser == token;
}
