using Steffi.Models;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class RectangleRenderable : Renderable
{
	private readonly Shape _shape;
	private readonly int _width;
	private readonly int _height;
	private readonly string? _rx;
	private readonly string? _ry;

	public RectangleRenderable(int x, int y, Shape shape, int width, int height, string? rx = null, string? ry = null)
	{
		X = x;
		Y = y;
		_shape = shape;
		_width = width;
		_height = height;
		_rx = rx;
		_ry = ry;
	}

	public override (XElement Element, int Width, int Height) Render() => (
		Element: SvgBuilder.Rect(X ?? 0, Y ?? 0, _width, _height,
			fillAndStroke: _shape,
			rx: _rx,
			ry: _ry), _width, _height);
}