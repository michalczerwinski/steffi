namespace Steffi.Parsers.Model;

public class Graph : SteffiObject, IParentObject, INamedObject
{
	public required string Name { get; set; }

	public List<SteffiObject> Children { get; } = [];
}
