using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class FillAndStrokeProperties : IFillAndStrokeProperties
{
	public string? Fill { get; set; }

	public string? FillOpacity { get; set; }

	public string? FillRule { get; set; }

	public string? Stroke { get; set; }

	public string? StrokeWidth { get; set; }

	public string? StrokeOpacity { get; set; }
	public string? StrokeLineCap { get; set; }
}

