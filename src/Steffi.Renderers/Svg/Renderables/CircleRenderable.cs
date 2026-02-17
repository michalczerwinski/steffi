using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class CircleRenderable(decimal x, decimal y, decimal r,
	IFillAndStroke fillAndStroke) : Renderable(x, y)
{
	public override (XElement Element, decimal Width, decimal Height) Render() => (
		Element: SvgBuilder.Circle(
			cx: (X ?? 0) + r,
			cy: (Y ?? 0) + r,
			r: r,
			fillAndStroke: fillAndStroke), 2 * r, 2 * r);
}
