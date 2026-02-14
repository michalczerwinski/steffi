using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class Text : SteffiObject, INamedObject, IChildObject
{
	public required string Name { get; set; }
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }
	public string? Spans { get; set; }
	public string? FontFamily { get; set; }
	public int? FontSize { get; set; }
	public string? FontColor { get; set; }
}