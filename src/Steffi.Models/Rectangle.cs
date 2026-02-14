using Steffi.Models.Attributes;
using Steffi.Models.Containers;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Rectangle : SteffiObject, INamedObject, IChildObject
{
	public required string Name { get; set; }
	public required IParentObject Parent { get; set; }
	public required ParentContainerProperties ParentProperties { get; set; }

	[GenerateModelBuilderSetter]
	public int Width { get; set; }

	[GenerateModelBuilderSetter]
	public int Height { get; set; }

	[GenerateModelBuilderSetter]
	public string? Fill { get; set; }

	[GenerateModelBuilderSetter]
	public string? Stroke { get; set; }

	[GenerateModelBuilderSetter]
	public string? StrokeWidth { get; set; }

	[GenerateModelBuilderSetter]
	public string? Rx { get; set; }

	[GenerateModelBuilderSetter]
	public string? Ry { get; set; }

}