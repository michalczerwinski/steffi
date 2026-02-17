
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Ellipse : Shape
{
	[GenerateModelBuilderSetter]
	public decimal Rx { get; set; }

	[GenerateModelBuilderSetter]
	public decimal Ry { get; set; }
}
