using Steffi.Renderers.Helpers;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class TextLine(string text, int fontSize = 20, int margin = 5) : Renderable
{
	public override (XElement Element, int Width, int Height) Render(int x, int y)
	{
		var (textWidth, textHeight) = TextMeasurementHelper.MeasureText(text, fontSize);

		var element = new XElement(SvgNamespace + "text",
				new XAttribute("x", x + textWidth / 2 + margin),
				new XAttribute("y", y + textHeight / 2),
				new XAttribute("font-size", fontSize),
				new XAttribute("text-anchor", "middle"),
				new XAttribute("dominant-baseline", "central"),
				new XAttribute("fill", "black"),
				text);

		return (element, textWidth + 2 * margin, textHeight + 2 * margin);
	}
}
