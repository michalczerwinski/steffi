
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Rectangle : Shape
{
	[GenerateModelBuilderSetter]
	public int Width { get; set; }

	[GenerateModelBuilderSetter]
	public int Height { get; set; }

	[GenerateModelBuilderSetter]
	public decimal? Rx { get; set; }

	[GenerateModelBuilderSetter]
	public decimal? Ry { get; set; }
}