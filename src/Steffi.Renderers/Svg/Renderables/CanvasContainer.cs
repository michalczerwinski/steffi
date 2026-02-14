using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class CanvasContainer(IList<Renderable> renderables, int padding = 5, int? width = null, int? height = null) : Renderable
{
	private readonly int? _width = width;
	private readonly int? _height = height;

	public override (XElement Element, int Width, int Height) Render()
	{
		var childRenders = new List<XElement>();

		int maxWidth = 0;
		int maxHeight = 0;

		for (int i = 0; i < renderables.Count; i++)
		{
			Renderable? child = renderables[i];
			if (child?.X == null || child.Y == null)
			{
				throw new InvalidOperationException("Object inside absolute container needs to have position set");
			}

			var childRender = child.Render();

			if (maxWidth < child.X.Value + childRender.Width + padding)
			{
				maxWidth = child.X.Value + childRender.Width + padding;
			}

			if (maxHeight < child.Y.Value + childRender.Height + padding)
			{
				maxHeight = child.Y.Value + childRender.Height + padding;
			}
			childRenders.Add(childRender.Element);
		}

		var render = new XElement(SvgNamespace + "g",
			(X != 0 || Y != 0) ? new XAttribute("transform", $"translate({X ?? 0}, {Y ?? 0})") : null,
			childRenders
		);

		return (render, _width ?? maxWidth, _height ?? maxHeight);
	}
}
