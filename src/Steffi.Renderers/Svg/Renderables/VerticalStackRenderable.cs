using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class VerticalStackRenderable(IList<Renderable> children, int padding = 5, int spacing = 3, bool includeBorder = true)
	: ContainerRenderable(children, padding, spacing, includeBorder)
{
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
				positionY += Spacing;
			}

			if (width < childRender.Width)
			{
				width = childRender.Width;
			}
			childRenders.Add(childRender.Element);
		}

		InsertBorder(width + 2 * Padding, positionY + Padding, childRenders);

		var render = new XElement(SvgNamespace + "g",
			(X != 0 || Y != 0) ? new XAttribute("transform", $"translate({X}, {Y})") : null,
			childRenders
		);

		return (render, width + 2 * Padding, positionY + Padding);
	}
}