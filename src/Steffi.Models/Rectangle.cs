using Steffi.Models.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Rectangle : Shape
{
	[GenerateModelBuilderSetter]
	public int Width { get; set; }

	[GenerateModelBuilderSetter]
	public int Height { get; set; }

	[GenerateModelBuilderSetter]
	public string? Rx { get; set; }

	[GenerateModelBuilderSetter]
	public string? Ry { get; set; }
}