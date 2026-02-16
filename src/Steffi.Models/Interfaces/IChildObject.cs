namespace Steffi.Models.Interfaces;

using Steffi.Models.Containers.Properties;

public interface IChildObject
{
	IParentObject Parent { get; set; }

	ParentProperties ParentProperties { get; set; }
}
