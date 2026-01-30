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
                    if (!element.IsMatch(parsingContext.PeekNextToken(tokenIndex)))
                    {
                        matchedSegmentElements = [];
                        return -1;
                    }
                    valuesTemp[name] = [parsingContext.PeekNextToken(tokenIndex)];
                    tokenIndex++;
                    break;
                case Arity.OptionalOnce:
                    valuesTemp[name] = [];
                    if (element.IsMatch(parsingContext.PeekNextToken(tokenIndex)))
                    {
                        valuesTemp[name].Add(parsingContext.PeekNextToken(tokenIndex));
                        tokenIndex++;
                    }
                    break;
                case Arity.OptionalMultiple:
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
