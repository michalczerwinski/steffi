namespace Steffi.Models;

public class ObjectReference<TReferencedObject> where TReferencedObject : SteffiObject
{
	public required string Name { get; set; }

	public TReferencedObject? Object { get; set; }
}