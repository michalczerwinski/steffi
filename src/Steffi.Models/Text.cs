using Steffi.Models.Attributes;
using Steffi.Models.Containers;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Text : SteffiObject, INamedObject, IChildObject
{
	public required string Name { get; set; }
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	[GenerateModelBuilderSetter]
	public string? Spans { get; set; }

	[GenerateModelBuilderSetter]
	public string? FontFamily { get; set; }

	[GenerateModelBuilderSetter]
	public int? FontSize { get; set; }

	[GenerateModelBuilderSetter]
	public string? FontColor { get; set; }
}