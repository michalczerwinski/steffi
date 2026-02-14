using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable : Renderable
{
	private readonly int _width;
	private readonly int _height;
	private readonly string? _fill;
	private readonly string? _stroke;

	public RectangleRenderable(int x, int y, int width, int height, string? fill = null, string? stroke = null)
	{
		X = x;
		Y = y;
		_width = width;
		_height = height;
		_fill = fill;
		_stroke = stroke;
	}

	public override (XElement Element, int Width, int Height) Render()
		=> (Element: new XElement(SvgNamespace + "rect",
			new XAttribute("x", X ?? 0),
			new XAttribute("y", Y ?? 0),
			new XAttribute("width", _width),
			new XAttribute("height", _height),
			new XAttribute("fill", _fill ?? "white"),
			new XAttribute("stroke", _stroke ?? "black")), _width, _height);
}