using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class HorizontalStackRenderable(IList<Renderable> children, int padding = 5, int spacing = 3, bool includeBorder = true)
	: ContainerRenderable(children, padding, spacing, includeBorder)
{
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
				positionX += Spacing;
			}

			if (height < childRender.Height)
			{
				height = childRender.Height;
			}
			childRenders.Add(childRender.Element);
		}

		InsertBorder(positionX + Padding, height + 2 * Padding, childRenders);

		var render = new XElement(SvgNamespace + "g",
			(X != 0 || Y != 0) ? new XAttribute("transform", $"translate({X}, {Y})") : null,
			childRenders
		);

		return (render, positionX + Padding, height + 2 * Padding);
	}
}
