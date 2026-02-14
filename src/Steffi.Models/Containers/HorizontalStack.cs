using Steffi.Models.Attributes;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class HorizontalStack : SteffiObject, INamedObject, IParentObject, IChildObject
{
	public required string Name { get; set; }

	public List<SteffiObject> Children { get; } = [];
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	public ParentContainerProperties CreateContainerProperties() => new EmptyContainerProperties();

	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }
}
