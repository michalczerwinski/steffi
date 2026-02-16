using Steffi.Models.Attributes;

namespace Steffi.Models;

public class Shape : SteffiObject
{
	[GenerateModelBuilderSetter]
	public string? Fill { get; set; }

	[GenerateModelBuilderSetter]
	public string? Stroke { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeWidth { get; set; }
}
