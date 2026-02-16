using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class ContainerRenderable(IList<Renderable> children, int padding = 0, int spacing = 3, bool inlcudeBorder = false, string? fill = null, string? stroke = null, string? strokeWidth = null)
	: Renderable
{
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;
	protected int Spacing { get; } = spacing;
	protected string? Fill { get; } = fill;
	protected string? Stroke { get; } = stroke;
	protected string? StrokeWidth { get; } = strokeWidth;

	protected void InsertBorder(int width, int height, List<XElement> childRenders)
	{
		// Always insert background rectangle, but use empty stroke when border is disabled
		var effectiveStroke = inlcudeBorder ? Stroke : "none";
		childRenders.Insert(0, new RectangleRenderable(0, 0, width, height, fill: Fill, stroke: effectiveStroke, strokeWidth: StrokeWidth).Render().Element);
	}
}
