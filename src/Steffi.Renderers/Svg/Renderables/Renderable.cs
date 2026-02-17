using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class Renderable(decimal? x = null, decimal? y = null)
{
	public decimal? X { get; set; } = x;

	public decimal? Y { get; set; } = y;

	public abstract (XElement Element, decimal Width, decimal Height) Render();
}
