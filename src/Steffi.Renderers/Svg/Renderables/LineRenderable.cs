using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class LineRenderable(int x1, int y1, int x2, int y2) : Renderable
{
	public override (XElement Element, int Width, int Height) Render()
	{
		var element = new XElement(SvgNamespace + "line",
				new XAttribute("x1", x1),
				new XAttribute("y1", y1),
				new XAttribute("x2", x2),
				new XAttribute("y2", y2),
				new XAttribute("stroke", "black"));

		return (element, 0, 0);
	}
}
