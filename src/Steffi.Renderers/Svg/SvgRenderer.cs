using Steffi.Models;
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
				new XAttribute("height", "100"),
				new XElement(svg + "circle",
					new XAttribute("cx", "50"),
					new XAttribute("cy", "50"),
					new XAttribute("r", "40")
				)
			)
		);
		var text = new XElement(svg + "text",
			new XAttribute("x", "50"),
			new XAttribute("y", "55"),
			new XAttribute("font-size", "20"),
			new XAttribute("text-anchor", "middle"),
			new XAttribute("fill", "black"),
			"Steffi");


		doc.Root!.Add(text);

		return doc.ToString();
	}
}
