using Steffi.Models.Builder.Attributes;
using Steffi.Models.Containers.Properties;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class Canvas : ContainerBase<CanvasParentProperties>
{

	[GenerateModelBuilderSetter]
	public int? Width { get; set; }

	[GenerateModelBuilderSetter]
	public int? Height { get; set; }

	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }

}