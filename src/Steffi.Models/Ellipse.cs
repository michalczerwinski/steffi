
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Ellipse : Shape
{
	[GenerateModelBuilderSetter]
	public int Rx { get; set; }

	[GenerateModelBuilderSetter]
	public int Ry { get; set; }
}
