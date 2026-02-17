using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable(decimal x, decimal y, int width, int height,
	IFillAndStroke fillAndStroke, decimal? rx = null, decimal? ry = null) : Renderable(x, y)
{
	public override (XElement Element, decimal Width, decimal Height) Render() => (
		Element: SvgBuilder.Rect(X ?? 0, Y ?? 0, width, height,
			fillAndStroke: fillAndStroke,
			rx: rx,
			ry: ry), width, height);
}