namespace Steffi.Models.Interfaces;

public interface IFillAndStroke
{
	public string? Fill { get; }
	public string? FillOpacity { get; }
	public string? FillRule { get; }
	public string? Stroke { get; }
	public string? StrokeWidth { get; }
	public string? StrokeOpacity { get; }
	public string? StrokeLineCap { get; }
}