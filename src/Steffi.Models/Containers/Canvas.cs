using Steffi.Models.Attributes;
using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

[GenerateModelBuilder]
public class Canvas : Shape, IParentObject
{
	public List<SteffiObject> Children { get; } = [];

	public ParentContainerProperties CreateContainerProperties() => new CanvasContainerProperties();

	[GenerateModelBuilderSetter]
	public int? Width { get; set; }

	[GenerateModelBuilderSetter]
	public int? Height { get; set; }

	[GenerateModelBuilderSetter]
	public bool? Border { get; set; }

	[GenerateModelBuilderSetter]
	public int? Padding { get; set; }

	public Canvas()
	{
		Stroke = "none";
	}
}
