using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable(int x, int y, int width, int height,
	IFillAndStroke fillAndStroke, string? rx = null, string? ry = null) : Renderable(x, y)
{
	public override (XElement Element, int Width, int Height) Render() => (
		Element: SvgBuilder.Rect(X ?? 0, Y ?? 0, width, height,
			fillAndStroke: fillAndStroke,
			rx: rx,
			ry: ry), width, height);
}