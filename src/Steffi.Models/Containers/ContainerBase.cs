using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Containers;

public class ContainerBase<TParentProperties> : Shape, IParentObject
	where TParentProperties : ParentProperties, new()
{
	public List<SteffiObject> Children { get; } = [];

	public ParentProperties CreateContainerProperties() => new TParentProperties();

	public ContainerBase()
	{
		Stroke = "none";
	}
}