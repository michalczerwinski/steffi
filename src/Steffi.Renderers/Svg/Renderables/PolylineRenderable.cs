using Steffi.Models;
using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class PolylineRenderable(int x, int y, List<Point2D> points,
	IFillAndStroke fillAndStroke) : Renderable(x, y)
{
	public override (XElement Element, int Width, int Height) Render()
	{
		var svgPoints = PointsHelper.ToSvgString(points, X ?? 0, Y ?? 0);
		var (width, height) = PointsHelper.MeasureBounds(points);
		return (SvgBuilder.Polyline(svgPoints, fillAndStroke), width, height);
	}
}
