namespace Steffi.Parsers.Parsing;

public record struct MatchedSegment(string Name, int StartIndex, int Length, int StartRow, int StartColumn)
{
	public ReadOnlySpan<char> GetValue(ParsingContext parsingContext) => parsingContext.Input.Slice(StartIndex, Length);

	public object GetPositionString() => $"({StartRow},{StartColumn}):";
}