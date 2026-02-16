using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class Renderable(int? x = null, int? y = null)
{
	public int? X { get; set; } = x;

	public int? Y { get; set; } = y;

	public abstract (XElement Element, int Width, int Height) Render();
}
