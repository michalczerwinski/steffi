using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables.Containers;

internal class CanvasRenderable(decimal? x, decimal? y, IList<Renderable> children, IFillAndStroke fillAndStroke, int padding = 0, int? width = null, int? height = null)
: ContainerRenderable(x, y, children, fillAndStroke, padding)
{
	private readonly decimal? _width = width;
	private readonly decimal? _height = height;
	private readonly int _padding = padding;

	public override (XElement Element, decimal Width, decimal Height) Render()
	{
		var childRenders = new List<XElement>();

		decimal maxWidth = 0;
		decimal maxHeight = 0;

		for (int i = 0; i < Children.Count; i++)
		{
			Renderable? child = Children[i];
			if (child?.X == null || child.Y == null)
			{
				throw new InvalidOperationException("Object inside absolute container needs to have position set");
			}

			child.X += _padding;
			child.Y += _padding;
			var childRender = child.Render();

			if (maxWidth < child.X.Value + childRender.Width + _padding)
			{
				maxWidth = child.X.Value + childRender.Width + _padding;
			}

			if (maxHeight < child.Y.Value + childRender.Height + _padding)
			{
				maxHeight = child.Y.Value + childRender.Height + _padding;
			}
			childRenders.Add(childRender.Element);
		}

		decimal finalWidth = _width ?? maxWidth;
		decimal finalHeight = _height ?? maxHeight;

		InsertBorder(finalWidth, finalHeight, childRenders);

		var render = SvgBuilder.Group(X, Y, childRenders);

		return (render, finalWidth, finalHeight);
	}
}
