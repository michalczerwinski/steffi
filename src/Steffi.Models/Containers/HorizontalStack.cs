using Steffi.Models.Attributes;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class HorizontalStack : SteffiObject, IParentObject, IChildObject
{
	public List<SteffiObject> Children { get; } = [];
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	public ParentContainerProperties CreateContainerProperties() => new EmptyContainerProperties();

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
