namespace Steffi.Models.Interfaces;

using Steffi.Models.Containers;

public interface IParentObject
{
	List<SteffiObject> Children { get; }

	ParentContainerProperties CreateContainerProperties();
}
