using Steffi.Models.Interfaces;

namespace Steffi.Models.Properties;

public class FillAndStrokeProperties : IFillAndStroke
{
	public string? Fill { get; set; }

	public string? FillOpacity { get; set; }

	public string? FillRule { get; set; }

	public string? Stroke { get; set; }

	public string? StrokeWidth { get; set; }

	public string? StrokeOpacity { get; set; }
	public string? StrokeLineCap { get; set; }
}

