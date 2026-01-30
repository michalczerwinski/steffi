namespace Steffi.Parsers.Parsing;

public record ParsedToken(int Id, Token TokenParser, int StartAt, int EndAt, int StartAtRow, int StartAtColumn)
{
	public override string ToString() => $"Id: {Id} Token: {TokenParser.Name} at: {GetPositionString()})";

	public string GetPositionString() => $"({StartAtRow}:{StartAtColumn})";
}
