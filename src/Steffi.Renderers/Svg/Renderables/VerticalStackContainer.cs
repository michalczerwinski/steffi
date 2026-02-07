using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class VerticalStackContainer(IList<Renderable> children, int padding = 5, int spacing = 3) : StackContainer(children, padding, spacing)
{
	public override (XElement Element, int Width, int Height) Render(int x = 0, int y = 0)
	{
		int positionY = Padding;
		int width = 0;

		var childRenders = new List<XElement>();

		for (int i = 0; i < Children.Count; i++)
		{
			Renderable? child = Children[i];
			var childRender = child.Render(Padding, positionY);
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

		childRenders.Insert(0, new Rectangle(width + 2 * Padding, positionY + Padding).Render(0, 0).Element);

		var render = new XElement(SvgNamespace + "g",
			(x != 0 || y != 0) ? new XAttribute("transform", $"translate({x}, {y})") : null,
			childRenders
		);

		return (render, width + 2 * Padding, positionY + Padding);
	}
}