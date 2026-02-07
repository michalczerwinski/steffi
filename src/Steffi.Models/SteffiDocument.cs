using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class SteffiDocument : SteffiObject, IParentObject
{
	public List<SteffiObject> Children { get; } = [];

	public LayoutType Layout { get; set; }
}
