using Steffi.Models.Attributes;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class Canvas : SteffiObject, INamedObject, IParentObject, IChildObject
{
	public required string Name { get; set; }

	public List<SteffiObject> Children { get; } = [];
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	public ParentContainerProperties CreateContainerProperties() => new CanvasContainerProperties();

	[GenerateModelBuilderSetter]
	public int? Width { get; set; }

	[GenerateModelBuilderSetter]
	public int? Height { get; set; }

	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }

	[GenerateModelBuilderSetter]
	public string? Fill { get; set; }

	[GenerateModelBuilderSetter]
	public string? Stroke { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeWidth { get; set; }
}
