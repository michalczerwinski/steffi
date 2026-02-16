using Steffi.Models.Builder.Attributes;
using Steffi.Models.Containers.Properties;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class VerticalStack : ContainerBase<EmptyParentProperties>
{
	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }

	[GenerateModelBuilderSetter]
	public int? Spacing { get; set; }
}