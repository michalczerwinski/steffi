
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Polyline : Shape
{
	[GenerateModelBuilderSetter]
	public List<Point2D> Points { get; set; } = [];
}
