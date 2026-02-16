using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables.Containers;

internal class VerticalStackRenderable(IList<Renderable> children, IFillAndStroke fillAndStroke, int padding = 0, int spacing = 3)
	: ContainerRenderable(children, fillAndStroke, padding)
{
	private readonly int _spacing = spacing;

	public override (XElement Element, int Width, int Height) Render()
	{
		int positionY = Padding;
		int width = 0;

		var childRenders = new List<XElement>();

		for (int i = 0; i < Children.Count; i++)
		{
			Renderable? child = Children[i];
			child.X = Padding;
			child.Y = positionY;
			var childRender = child.Render();
			positionY += childRender.Height;

			if (i != Children.Count - 1)
			{
				positionY += _spacing;
			}

			if (width < childRender.Width)
			{
				width = childRender.Width;
			}
			childRenders.Add(childRender.Element);
		}

		InsertBorder(width + 2 * Padding, positionY + Padding, childRenders);

		var render = SvgBuilder.Group(X, Y, childRenders);

		return (render, width + 2 * Padding, positionY + Padding);
	}
}