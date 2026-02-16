using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class ContainerRenderable(IList<Renderable> children, IFillAndStrokeProperties fillAndStroke, int padding = 0, bool inlcudeBorder = false)
	: Renderable
{
	protected IFillAndStrokeProperties FillAndStroke { get; } = fillAndStroke;
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;

	protected void InsertBorder(int width, int height, List<XElement> childRenders)
	{
		var effectiveStroke = inlcudeBorder ? FillAndStroke.Stroke : "none";
		childRenders.Insert(0, SvgBuilder.Rect(0, 0, width, height, FillAndStroke));
	}
}
