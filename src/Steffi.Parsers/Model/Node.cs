namespace Steffi.Parsers.Model;

public class Node : SteffiObject, INamedObject
{
	public required string Name { get; set; }

	public string Label { get; set; } = string.Empty;
}
