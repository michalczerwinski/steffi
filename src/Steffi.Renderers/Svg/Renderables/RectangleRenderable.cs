using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable : Renderable
{
	private readonly int _width;
	private readonly int _height;
	private readonly string? _fill;
	private readonly string? _fillOpacity;
	private readonly string? _fillRule;
	private readonly string? _stroke;
	private readonly string? _strokeWidth;
	private readonly string? _strokeOpacity;
	private readonly string? _strokeLineCap;
	private readonly string? _rx;
	private readonly string? _ry;

	public RectangleRenderable(int x, int y, int width, int height, string? fill = null, string? fillOpacity = null,
		string? fillRule = null, string? stroke = null, string? strokeWidth = null, string? strokeOpacity = null,
		string? strokeLineCap = null, string? rx = null, string? ry = null)
	{
		X = x;
		Y = y;
		_width = width;
		_height = height;
		_fill = fill;
		_fillOpacity = fillOpacity;
		_fillRule = fillRule;
		_stroke = stroke;
		_strokeWidth = strokeWidth;
		_strokeOpacity = strokeOpacity;
		_strokeLineCap = strokeLineCap;
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
			string.IsNullOrEmpty(_fillOpacity) ? null : new XAttribute("fill-opacity", _fillOpacity),
			string.IsNullOrEmpty(_fillRule) ? null : new XAttribute("fill-rule", _fillRule),
			string.IsNullOrEmpty(_rx) ? null : new XAttribute("rx", _rx),
			string.IsNullOrEmpty(_ry) ? null : new XAttribute("ry", _ry),
			new XAttribute("stroke", _stroke ?? "black"),
			string.IsNullOrEmpty(_strokeWidth) ? null : new XAttribute("stroke-width", _strokeWidth),
			string.IsNullOrEmpty(_strokeOpacity) ? null : new XAttribute("stroke-opacity", _strokeOpacity),
			string.IsNullOrEmpty(_strokeLineCap) ? null : new XAttribute("stroke-linecap", _strokeLineCap)), _width, _height);
}