using Steffi.Models.Builder.Attributes;
using Steffi.Models.Containers.Properties;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class HorizontalStack : ContainerBase<EmptyParentProperties>
{
	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }

	[GenerateModelBuilderSetter]
	public int? Spacing { get; set; }

}
