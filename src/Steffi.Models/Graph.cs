using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Graph : SteffiObject, IParentObject, INamedObject, ILabeledObject
{
	public required string Name { get; set; }

	public List<SteffiObject> Children { get; } = [];
	public LayoutType Layout { get; set; }

	public string? Label { get; set; }
	public string? FontColor { get; set; }
}
