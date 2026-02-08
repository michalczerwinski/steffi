using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class StackContainer(IList<Renderable> children, int padding = 5, int spacing = 3, bool inlcudeBorder = true) : Renderable
{
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;
	protected int Spacing { get; } = spacing;

	protected void InsertBorder(int width, int height, List<XElement> childRenders)
	{
		if (inlcudeBorder)
		{
			childRenders.Insert(0, new Rectangle(width, height).Render(0, 0).Element);
		}
	}
}
