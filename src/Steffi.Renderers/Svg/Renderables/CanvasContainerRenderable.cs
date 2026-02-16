using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class CanvasContainerRenderable(IList<Renderable> renderables, int padding = 0, int? width = null, int? height = null, bool includeBorder = false, string? fill = null, string? stroke = null, string? strokeWidth = null) : Renderable
{
	private readonly int? _width = width;
	private readonly int? _height = height;
	private readonly int _padding = padding;
	private readonly bool _includeBorder = includeBorder;
	private readonly string? _fill = fill;
	private readonly string? _stroke = stroke;
	private readonly string? _strokeWidth = strokeWidth;

	public override (XElement Element, int Width, int Height) Render()
	{
		var childRenders = new List<XElement>();

		int maxWidth = 0;
		int maxHeight = 0;

		for (int i = 0; i < renderables.Count; i++)
		{
			Renderable? child = renderables[i];
			if (child?.X == null || child.Y == null)
			{
				throw new InvalidOperationException("Object inside absolute container needs to have position set");
			}

			var childRender = child.Render();
			child.X += _padding;
			child.Y += _padding;

			if (maxWidth < child.X.Value + childRender.Width + _padding)
			{
				maxWidth = child.X.Value + childRender.Width + _padding;
			}

			if (maxHeight < child.Y.Value + childRender.Height + _padding)
			{
				maxHeight = child.Y.Value + childRender.Height + _padding;
			}
			childRenders.Add(childRender.Element);
		}

		var finalWidth = _width ?? maxWidth;
		var finalHeight = _height ?? maxHeight;

		// Always insert background rectangle, but use empty stroke when border is disabled
		var effectiveStroke = _includeBorder ? _stroke : "none";
		childRenders.Insert(0, new RectangleRenderable(0, 0, finalWidth, finalHeight, fill: _fill, stroke: effectiveStroke, strokeWidth: _strokeWidth).Render().Element);

		var render = new XElement(SvgNamespace + "g",
			(X != 0 || Y != 0) ? new XAttribute("transform", $"translate({X ?? 0}, {Y ?? 0})") : null,
			childRenders
		);

		return (render, finalWidth, finalHeight);
	}
}
