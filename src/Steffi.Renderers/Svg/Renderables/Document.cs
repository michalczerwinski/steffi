using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class Document(Renderable main)
{
	public XDocument Render() => new XDocument(
		SvgBuilder.Svg(
			main.Render().Width,
			main.Render().Height,
			main.Render().Element
		)
	);
}