using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class LineRenderable(int x1, int y1, int x2, int y2) : Renderable
{
	public override (XElement Element, int Width, int Height) Render()
	{
		return (SvgBuilder.Line(x1, y1, x2, y2), 0, 0);
	}
}
