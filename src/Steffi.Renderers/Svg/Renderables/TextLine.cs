using Steffi.Renderers.Helpers;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg.Renderables;

internal class TextLine(string text, string fontFamily, string fontColor = "black", int fontSize = 20, int margin = 5)
	: Renderable
{
	public override (XElement Element, decimal Width, decimal Height) Render()
	{
		var (textWidth, textHeight) = TextMeasurementHelper.MeasureText(text, fontSize, fontFamily);

		var element = SvgBuilder.Text(
			X!.Value + textWidth / 2 + margin,
			Y!.Value + textHeight / 2,
			fontSize, fontFamily,
			"middle", "central",
			fontColor, text);

		return (element, textWidth + 2 * margin, textHeight + 2 * margin);
	}
}
