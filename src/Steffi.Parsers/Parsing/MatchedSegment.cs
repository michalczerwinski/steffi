namespace Steffi.Parsers.Parsing;

public ref struct MatchedSegment(ReadOnlySpan<char> name, int startIndex, int length, int startRow, int startColumn, ReadOnlySpan<char> value)
{
	public ReadOnlySpan<char> Name { get; } = name;

	public int StartIndex { get; } = startIndex;

	public int Length { get; } = length;

	public int StartRow { get; } = startRow;

	public int StartColumn { get; } = startColumn;

	public ReadOnlySpan<char> Value { get; } = value;

	public ReadOnlySpan<char> GetValueOrDefault(Func<ReadOnlySpan<char>> defaultValueFactoryFunc)
		=> Length == 0
		? defaultValueFactoryFunc.Invoke()
		: Value;

	public object GetPositionString() => $"({StartRow},{StartColumn}):";

	public string CreateError(ReadOnlySpan<char> error) => $"{GetPositionString()} {error}";
}