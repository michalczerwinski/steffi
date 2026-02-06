using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class Document(Renderable renderable)
{
	public XDocument Render()
	{
		var rendered = renderable.Render(0, 0);

		return new XDocument(
			new XElement(Renderable.SvgNamespace + "svg",
				new XAttribute("width", rendered.Width),
				new XAttribute("height", rendered.Height),
				rendered.Element
			)
		);
	}
}