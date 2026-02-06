using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class Rectangle(int width, int height) : Renderable
{
	public override (XElement Element, int Width, int Height) Render(int x, int y)
		=> (Element: new XElement(SvgNamespace + "rect",
			new XAttribute("x", 0),
			new XAttribute("y", 0),
			new XAttribute("width", width),
			new XAttribute("height", height),
			new XAttribute("fill", "white"),
			new XAttribute("stroke", "black")), width, height);
}