using Steffi.Models;
using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg;

public class SvgRenderer : ISteffiDocumentRenderer
{
	public string RenderDocument(SteffiDocument document)
	{

		XNamespace svg = "http://www.w3.org/2000/svg";
		var doc = new XDocument(
			new XElement(svg + "svg",
				new XAttribute("width", "100"),
				new XAttribute("height", "100")
			)
		);

		int y = 55;
		foreach (var steffiObject in document.Children)
		{
			var circle = new XElement(svg + "circle",
				new XAttribute("cx", "50"),
				new XAttribute("cy", y),
				new XAttribute("r", "40")
			);
			doc.Root!.Add(circle);

			var text = new XElement(svg + "text",
				new XAttribute("x", "50"),
				new XAttribute("y", y),
				new XAttribute("font-size", "20"),
				new XAttribute("text-anchor", "middle"),
				new XAttribute("fill", "white"),
				steffiObject is INamedObject namedObject ? namedObject.Name : "(no name)");
			doc.Root!.Add(text);

			y += 55;
		}

		doc.Root!.Attribute("height")!.Value = y.ToString();

		return doc.ToString();
	}
}
