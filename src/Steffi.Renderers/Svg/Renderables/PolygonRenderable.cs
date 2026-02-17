using Steffi.Models;
using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class PolygonRenderable(decimal x, decimal y, List<Point2D> points,
	IFillAndStroke fillAndStroke) : Renderable(x, y)
{
	public override (XElement Element, decimal Width, decimal Height) Render()
	{
		var svgPoints = PointsHelper.ToSvgString(points, X ?? 0, Y ?? 0);
		var (width, height) = PointsHelper.MeasureBounds(points);
		return (SvgBuilder.Polygon(svgPoints, fillAndStroke), width, height);
	}
}
