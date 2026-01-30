namespace Steffi.Parsers.Model;

public class SteffiDocument : SteffiObject, IParentObject
{
	public List<SteffiObject> Children { get; } = [];
}
