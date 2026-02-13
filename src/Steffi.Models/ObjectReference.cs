namespace Steffi.Models;

public class ObjectReference
{
	public required string Name { get; set; }

	public SteffiObject? Object { get; set; }
}