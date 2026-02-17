
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Circle : Shape
{
	[GenerateModelBuilderSetter]
	public decimal R { get; set; }
}
