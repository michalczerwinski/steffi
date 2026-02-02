using Steffi.Models;

namespace Steffi.Models.Interfaces;

public interface IParentObject
{
	List<SteffiObject> Children { get; }
}
