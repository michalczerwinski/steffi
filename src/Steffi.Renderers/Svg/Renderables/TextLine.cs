using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class TextLine(string text, int fontSize = 20, int margin = 5) : Renderable
{
	public override (XElement Element, int Width, int Height) Render(int x, int y)
	{
		MeasureText(text, out var textWidth, out var textHeight);

		var element = new XElement(SvgNamespace + "text",
				new XAttribute("x", x + textWidth / 2),
				new XAttribute("y", y + textHeight / 2),
				new XAttribute("font-size", fontSize),
				new XAttribute("text-anchor", "middle"),
				new XAttribute("dominant-baseline", "central"),
				new XAttribute("fill", "black"),
				text);

		return (element, textWidth + 2 * margin, textHeight + 2 * margin);
	}
	private static void MeasureText(string text, out int textWidth, out int textHeight)
	{
		textWidth = text.Length * 11;
		textHeight = 20;
	}
}
