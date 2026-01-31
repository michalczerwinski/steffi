namespace Steffi.Parsers.Parsing.TokenSequences;

public class TokenSequence
{
	private readonly List<(string, TokenSequenceElement, Arity)> segments = [];

	public TokenSequence AddSegment(string name, TokenSequenceElement element, Arity arity)
	{
		segments.Add((name, element, arity));

		return this;
	}

	public int Match(ParsingContext parsingContext, out Dictionary<string, List<ParsedToken>> matchedSegmentElements)
	{
		var tokenIndex = 0;
		var valuesTemp = new Dictionary<string, List<ParsedToken>>();

		foreach (var (name, element, segmentType) in segments)
		{
			switch (segmentType)
			{
				case Arity.RequiredOnce:
					if (!parsingContext.Tokens.PeekNext(out var token, tokenIndex) || !element.IsMatch(token))
					{
						matchedSegmentElements = [];
						return -1;
					}
					valuesTemp[name] = [token];
					tokenIndex++;
					break;
				case Arity.OptionalOnce:
					valuesTemp[name] = [];
					if (parsingContext.Tokens.PeekNext(out token, tokenIndex) && element.IsMatch(token))
					{
						valuesTemp[name].Add(token);
						tokenIndex++;
					}
					break;
				case Arity.OptionalMultiple:
					valuesTemp[name] = [];
					while (parsingContext.Tokens.PeekNext(out token, tokenIndex) && element.IsMatch(token))
					{
						valuesTemp[name].Add(token);
						tokenIndex++;
					}
					break;
				default:
					throw new NotSupportedException($"Segment type '{segmentType}' is not supported.");
			}
		}

		matchedSegmentElements = valuesTemp;

		return tokenIndex;
	}
}
