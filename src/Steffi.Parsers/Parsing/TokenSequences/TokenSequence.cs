namespace Steffi.Parsers.Parsing.TokenSequences;

public class TokenSequence
{
	private List<(string, TokenSequenceElement, SegmentType)> segments = new();

	public TokenSequence AddSegment(string name, TokenSequenceElement element, SegmentType segmentType)
	{
		segments.Add((name, element, segmentType));

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
				case SegmentType.RequiredOnce:
					if (!element.IsMatch(parsingContext.PeekNextToken(tokenIndex)))
					{
						matchedSegmentElements = [];
						return -1;
					}
					valuesTemp[name] = new List<ParsedToken> { parsingContext.PeekNextToken(tokenIndex) };
					tokenIndex++;
					break;
				case SegmentType.OptionalOnce:
					valuesTemp[name] = [];
					if (element.IsMatch(parsingContext.PeekNextToken(tokenIndex)))
					{
						valuesTemp[name].Add(parsingContext.PeekNextToken(tokenIndex));
						tokenIndex++;
					}
					break;
				case SegmentType.OptionalMultiple:
					valuesTemp[name] = [];
					while (element.IsMatch(parsingContext.PeekNextToken(tokenIndex)))
					{
						valuesTemp[name].Add(parsingContext.PeekNextToken(tokenIndex));
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