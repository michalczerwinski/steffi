using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable : Renderable
{
	private readonly int _width;
	private readonly int _height;
	private readonly string? _fill;
	private readonly string? _stroke;
	private readonly string? _rx;
	private readonly string? _ry;

	public RectangleRenderable(int x, int y, int width, int height, string? fill = null, string? stroke = null,
		string? rx = null, string? ry = null)
	{
		X = x;
		Y = y;
		_width = width;
		_height = height;
		_fill = fill;
		_stroke = stroke;
		_rx = rx;
		_ry = ry;
	}

	public override (XElement Element, int Width, int Height) Render()
		=> (Element: new XElement(SvgNamespace + "rect",
			new XAttribute("x", X ?? 0),
			new XAttribute("y", Y ?? 0),
			new XAttribute("width", _width),
			new XAttribute("height", _height),
			new XAttribute("fill", _fill ?? "white"),
			string.IsNullOrEmpty(_rx) ? null : new XAttribute("rx", _rx),
			string.IsNullOrEmpty(_ry) ? null : new XAttribute("ry", _ry),
			new XAttribute("stroke", _stroke ?? "black")), _width, _height);
}