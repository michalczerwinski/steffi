namespace Steffi.Parsers.Parsing.TokenSequences;

public class OneOfManyTokens(Token[] tokens) : TokenSequenceElement
{
	public override bool IsMatch(ParsedToken parsedToken) => tokens.Any(t => t == parsedToken.TokenParser);
}

