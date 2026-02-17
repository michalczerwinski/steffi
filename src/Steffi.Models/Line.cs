
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Line : Shape
{
	[GenerateModelBuilderSetter]
	public decimal? X1 { get; set; }
	[GenerateModelBuilderSetter]
	public decimal? Y1 { get; set; }
	[GenerateModelBuilderSetter]
	public decimal X2 { get; set; }
	[GenerateModelBuilderSetter]
	public decimal Y2 { get; set; }
}