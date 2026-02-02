namespace Steffi.Parsers.Parsing;

public ref struct MatchedSegment(ReadOnlySpan<char> name, int startIndex, int length, int startRow, int startColumn)
{
	public ReadOnlySpan<char> Name { get; } = name;

	public int StartIndex { get; } = startIndex;

	public int Length { get; } = length;

	public int StartRow { get; } = startRow;

	public int StartColumn { get; } = startColumn;

	public ReadOnlySpan<char> GetValue(ParsingContext parsingContext) => parsingContext.Input.Slice(StartIndex, Length);

	public ReadOnlySpan<char> GetValueOrDefault(ParsingContext parsingContext, Func<ReadOnlySpan<char>> defaultValueFactoryFunc)
		=> Length == 0
		? defaultValueFactoryFunc.Invoke()
		: parsingContext.Input.Slice(StartIndex, Length);

	public object GetPositionString() => $"({StartRow},{StartColumn}):";

	public string CreateError(ReadOnlySpan<char> error) => $"{GetPositionString()} {error}";
}