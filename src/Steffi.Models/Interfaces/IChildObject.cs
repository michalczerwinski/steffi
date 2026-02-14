namespace Steffi.Models.Interfaces;

using Steffi.Models.Containers;

public interface IChildObject
{
	IParentObject Parent { get; set; }

	ParentContainerProperties ParentProperties { get; set; }
}
