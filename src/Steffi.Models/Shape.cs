using Steffi.Models.Attributes;
using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Shape : SteffiObject, IChildObject
{
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

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
