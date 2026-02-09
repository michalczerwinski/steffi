using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Edge : SteffiObject, INamedObject
{
	public required string Name { get; set; }
	public SteffiObject? From { get; set; }
	public SteffiObject? To { get; set; }
}
