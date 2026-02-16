using Steffi.Models.Attributes;
using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class VerticalStack : Shape, IParentObject
{
	public List<SteffiObject> Children { get; } = [];

	public ParentContainerProperties CreateContainerProperties() => new EmptyContainerProperties();

	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }
}