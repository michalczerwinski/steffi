using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class Document(Renderable main)
{
	public XDocument Render()
	{
		var mainRender = main.Render();

		return new XDocument(
		SvgBuilder.Svg(
			mainRender.Width,
			mainRender.Height,
			mainRender.Element
		)
	);
	}
}