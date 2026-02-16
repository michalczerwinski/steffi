namespace Steffi.Models.Interfaces;

using Steffi.Models.Containers.Properties;

public interface IParentObject
{
	List<SteffiObject> Children { get; }

	ParentProperties CreateContainerProperties();
}
