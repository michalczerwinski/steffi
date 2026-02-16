using Steffi.Models.Builder.Attributes;
using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Shape : SteffiObject, IChildObject, IFillAndStrokeProperties
{
	public required IParentObject Parent { get; set; }
	public required ParentProperties ParentProperties { get; set; }

	[GenerateModelBuilderSetter]
	public string? Fill { get; set; } = "white";

	[GenerateModelBuilderSetter]
	public string? FillOpacity { get; set; }

	[GenerateModelBuilderSetter]
	public string? FillRule { get; set; }

	[GenerateModelBuilderSetter]
	public string? Stroke { get; set; } = "black";

	[GenerateModelBuilderSetter]
	public string? StrokeWidth { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeOpacity { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeLineCap { get; set; }
}
