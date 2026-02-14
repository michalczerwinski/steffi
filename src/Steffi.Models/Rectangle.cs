using Steffi.Models.Containers;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Rectangle : SteffiObject, INamedObject, IChildObject
{
	public required string Name { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }
}