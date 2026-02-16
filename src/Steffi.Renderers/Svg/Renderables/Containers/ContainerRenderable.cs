using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables.Containers;

internal abstract class ContainerRenderable(int? x, int? y, IList<Renderable> children, IFillAndStroke fillAndStroke, int padding = 0)
	: Renderable(x, y)
{
	protected IFillAndStroke FillAndStroke { get; } = fillAndStroke;
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;

	protected void InsertBorder(int width, int height, List<XElement> childRenders)
	{
		childRenders.Insert(0, SvgBuilder.Rect(0, 0, width, height, FillAndStroke));
	}
}
