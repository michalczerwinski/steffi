
using Steffi.Models.Builder.Attributes;

namespace Steffi.Models;

[GenerateModelBuilder]
public class Polygon : Shape
{
	[GenerateModelBuilderSetter]
	public List<Point2D> Points { get; set; } = [];
}
