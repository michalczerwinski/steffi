using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class LineRenderable(decimal x1, decimal y1, decimal x2, decimal y2, IFillAndStroke fillAndStroke) : Renderable
{
	public override (XElement Element, decimal Width, decimal Height) Render()
	{
		return (SvgBuilder.Line(x1, y1, x2, y2, fillAndStroke), 0, 0);
	}
}
