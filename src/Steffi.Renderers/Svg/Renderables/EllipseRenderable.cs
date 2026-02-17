using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class EllipseRenderable(decimal x, decimal y, decimal rx, decimal ry,
	IFillAndStroke fillAndStroke) : Renderable(x, y)
{
	public override (XElement Element, decimal Width, decimal Height) Render() => (
		Element: SvgBuilder.Ellipse(
			cx: (X ?? 0) + rx,
			cy: (Y ?? 0) + ry,
			rx: rx,
			ry: ry,
			fillAndStroke: fillAndStroke), 2 * rx, 2 * ry);
}
