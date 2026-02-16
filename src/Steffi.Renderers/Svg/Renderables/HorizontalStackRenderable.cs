using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class HorizontalStackRenderable(IList<Renderable> children, IFillAndStrokeProperties fillAndStroke, int padding = 0, int spacing = 3, bool includeBorder = false)
	: ContainerRenderable(children, fillAndStroke, padding, includeBorder)
{
	private readonly int _spacing = spacing;

	public override (XElement Element, int Width, int Height) Render()
	{
		int positionX = Padding;
		int height = 0;

		var childRenders = new List<XElement>();

		for (int i = 0; i < Children.Count; i++)
		{
			Renderable? child = Children[i];
			child.X = positionX;
			child.Y = Padding;
			var childRender = child.Render();
			positionX += childRender.Width;

			if (i != Children.Count - 1)
			{
				positionX += _spacing;
			}

			if (height < childRender.Height)
			{
				height = childRender.Height;
			}
			childRenders.Add(childRender.Element);
		}

		InsertBorder(positionX + Padding, height + 2 * Padding, childRenders);

		var render = SvgBuilder.Group(X, Y, childRenders);

		return (render, positionX + Padding, height + 2 * Padding);
	}
}
