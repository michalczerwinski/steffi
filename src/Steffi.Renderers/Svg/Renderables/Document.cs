using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class Document(Renderable main)
{
	public XDocument Render() => new XDocument(
			new XElement(Renderable.SvgNamespace + "svg",
				new XAttribute("width", main.Render().Width),
				new XAttribute("height", main.Render().Height),
				main.Render().Element
			)
		);
}