using Steffi.Models.Interfaces;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg;

internal static class SvgBuilder
{
	private static readonly XNamespace SvgNamespace = "http://www.w3.org/2000/svg";

	private static IEnumerable<XAttribute?> GetFillAndStrokeAttributes(IFillAndStroke fillAndStroke) =>
	[
		string.IsNullOrEmpty(fillAndStroke.Fill) ? null : new XAttribute("fill", fillAndStroke.Fill),
		string.IsNullOrEmpty(fillAndStroke.FillOpacity) ? null : new XAttribute("fill-opacity", fillAndStroke.FillOpacity),
		string.IsNullOrEmpty(fillAndStroke.FillRule) ? null : new XAttribute("fill-rule", fillAndStroke.FillRule),
		string.IsNullOrEmpty(fillAndStroke.Stroke) ? null : new XAttribute("stroke", fillAndStroke.Stroke),
		string.IsNullOrEmpty(fillAndStroke.StrokeWidth) ? null : new XAttribute("stroke-width", fillAndStroke.StrokeWidth),
		string.IsNullOrEmpty(fillAndStroke.StrokeOpacity) ? null : new XAttribute("stroke-opacity", fillAndStroke.StrokeOpacity),
		string.IsNullOrEmpty(fillAndStroke.StrokeLineCap) ? null : new XAttribute("stroke-linecap", fillAndStroke.StrokeLineCap)
	];

	internal static XElement Rect(
		int x, int y, int width, int height,
		IFillAndStroke fillAndStroke,
		string? rx = null, string? ry = null)
		=> new(SvgNamespace + "rect",
			new XAttribute("x", x),
			new XAttribute("y", y),
			new XAttribute("width", width),
			new XAttribute("height", height),
			string.IsNullOrEmpty(rx) ? null : new XAttribute("rx", rx),
			string.IsNullOrEmpty(ry) ? null : new XAttribute("ry", ry),
			GetFillAndStrokeAttributes(fillAndStroke));

	internal static XElement Circle(
		int cx, int cy, int r,
		IFillAndStroke fillAndStroke)
		=> new(SvgNamespace + "circle",
			new XAttribute("cx", cx),
			new XAttribute("cy", cy),
			new XAttribute("r", r),
			GetFillAndStrokeAttributes(fillAndStroke));

	internal static XElement Ellipse(
		int cx, int cy, int rx, int ry,
		IFillAndStroke fillAndStroke)
		=> new(SvgNamespace + "ellipse",
			new XAttribute("cx", cx),
			new XAttribute("cy", cy),
			new XAttribute("rx", rx),
			new XAttribute("ry", ry),
			GetFillAndStrokeAttributes(fillAndStroke));

	internal static XElement Polygon(
		string points,
		IFillAndStroke fillAndStroke)
		=> new(SvgNamespace + "polygon",
			new XAttribute("points", points),
			GetFillAndStrokeAttributes(fillAndStroke));

	internal static XElement Polyline(
		string points,
		IFillAndStroke fillAndStroke)
		=> new(SvgNamespace + "polyline",
			new XAttribute("points", points),
			GetFillAndStrokeAttributes(fillAndStroke));

	internal static XElement Group(int? x, int? y, List<XElement> children)
		=> new(SvgNamespace + "g",
			(x != 0 || y != 0)
				? new XAttribute("transform", $"translate({x}, {y})")
				: null,
			children);

	internal static XElement Line(int x1, int y1, int x2, int y2, string stroke = "black")
		=> new(SvgNamespace + "line",
			new XAttribute("x1", x1),
			new XAttribute("y1", y1),
			new XAttribute("x2", x2),
			new XAttribute("y2", y2),
			new XAttribute("stroke", stroke));

	internal static XElement Text(int x, int y, int fontSize, string fontFamily, string textAnchor, string dominantBaseline, string fill, string text)
		=> new(SvgNamespace + "text",
			new XAttribute("x", x),
			new XAttribute("y", y),
			new XAttribute("font-size", fontSize),
			new XAttribute("font-family", fontFamily),
			new XAttribute("text-anchor", textAnchor),
			new XAttribute("dominant-baseline", dominantBaseline),
			new XAttribute("fill", fill),
			text);

	internal static XElement Svg(int width, int height, XElement content)
		=> new(SvgNamespace + "svg",
			new XAttribute("width", width),
			new XAttribute("height", height),
			content);
}
