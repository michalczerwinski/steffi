using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal abstract class Renderable
{
	internal static readonly XNamespace SvgNamespace = "http://www.w3.org/2000/svg";

	public abstract (XElement Element, int Width, int Height) Render(int x, int y);
}