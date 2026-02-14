using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class ContainerRenderable(IList<Renderable> children, int padding = 5, int spacing = 3, bool inlcudeBorder = true)
	: Renderable
{
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;
	protected int Spacing { get; } = spacing;

	protected void InsertBorder(int width, int height, List<XElement> childRenders)
	{
		if (inlcudeBorder)
		{
			childRenders.Insert(0, new RectangleRenderable(0, 0, width, height).Render().Element);
		}
	}
}
