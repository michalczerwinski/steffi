using Steffi.Models.Attributes;

namespace Steffi.Models;

public class Shape : SteffiObject
{
	[GenerateModelBuilderSetter]
	public string? Fill { get; set; }

	[GenerateModelBuilderSetter]
	public string? FillOpacity { get; set; }

	[GenerateModelBuilderSetter]
	public string? FillRule { get; set; }

	[GenerateModelBuilderSetter]
	public string? Stroke { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeWidth { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeOpacity { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeLineCap { get; set; }
}
