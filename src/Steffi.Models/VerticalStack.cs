using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class VerticalStack : SteffiObject, INamedObject, IParentObject, IChildObject
{
	public required string Name { get; set; }

	public LayoutType Layout { get; set; } = LayoutType.Vertical;
	public List<SteffiObject> Children { get; } = [];
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	public ParentContainerProperties CreateContainerProperties() => new EmptyContainerProperties();
}