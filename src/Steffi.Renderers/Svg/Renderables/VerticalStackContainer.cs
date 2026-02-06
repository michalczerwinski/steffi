using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class VerticalStackContainer(IList<Renderable> children, int padding = 5, int spacing = 3) : Renderable
{
	public override (XElement Element, int Width, int Height) Render(int x, int y)
	{
		int positionY = padding;
		int width = 0;

		var childRenders = new List<XElement>();

		for (int i = 0; i < children.Count; i++)
		{
			Renderable? child = children[i];
			var childRender = child.Render(padding, positionY);
			positionY += childRender.Height;

			if (i != children.Count - 1)
			{
				positionY += spacing;
			}

			if (width < childRender.Width)
			{
				width = childRender.Width;
			}
			childRenders.Add(childRender.Element);
		}

		childRenders.Insert(0, new Rectangle(width + 2 * padding, positionY + padding).Render(0, 0).Element);

		var render = new XElement(SvgNamespace + "g",
			new XAttribute("transform", $"translate({x}, {y})"),
			childRenders
		);

		return (render, width + 2 * padding, positionY + padding);
	}
}