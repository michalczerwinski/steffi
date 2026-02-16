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
		=> (Element: SvgBuilder.Rect(X ?? 0, Y ?? 0, _width, _height,
			fill: _fill, fillOpacity: _fillOpacity, fillRule: _fillRule,
			stroke: _stroke, strokeWidth: _strokeWidth, strokeOpacity: _strokeOpacity, strokeLineCap: _strokeLineCap,
			rx: _rx, ry: _ry), _width, _height);
}