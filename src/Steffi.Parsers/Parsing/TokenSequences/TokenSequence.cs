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
		var position = 0;
		var valuesTemp = new Dictionary<string, List<ParsedToken>>();

		foreach (var (name, element, segmentType) in segments)
		{
			switch (segmentType)
			{
				case Arity.RequiredOnce:
					if (!parsingContext.Tokens.PeekNext(out var token, position) || !element.IsMatch(token))
					{
						matchedSegmentElements = [];
						return -1;
					}
					valuesTemp[name] = [token];
					position++;
					break;
				case Arity.OptionalOnce:
					valuesTemp[name] = [];
					if (parsingContext.Tokens.PeekNext(out token, position) && element.IsMatch(token))
					{
						valuesTemp[name].Add(token);
						position++;
					}
					break;
				case Arity.OptionalMultiple:
					valuesTemp[name] = [];
					while (parsingContext.Tokens.PeekNext(out token, position) && element.IsMatch(token))
					{
						valuesTemp[name].Add(token);
						position++;
					}
					break;
				case Arity.AtLeastOnce:
					if (!parsingContext.Tokens.PeekNext(out token, position) || !element.IsMatch(token))
					{
						matchedSegmentElements = [];
						return -1;
					}
					valuesTemp[name] = [token];
					position++;
					while (parsingContext.Tokens.PeekNext(out token, position) && element.IsMatch(token))
					{
						valuesTemp[name].Add(token);
						position++;
					}
					break;
				default:
					throw new NotSupportedException($"Segment type '{segmentType}' is not supported.");
			}
		}

		matchedSegmentElements = valuesTemp;

		return position;
	}
}
