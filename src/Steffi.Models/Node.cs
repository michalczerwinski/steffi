using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Node : SteffiObject, INamedObject, ILabeledObject
{
	public required string Name { get; set; }

	public string? Label { get; set; }

	public string? FontColor { get; set; }
}
