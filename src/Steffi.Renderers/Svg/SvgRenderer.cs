using Steffi.Models;
using Steffi.Models.Containers;
using Steffi.Models.Interfaces;
using Steffi.Renderers.Svg.Renderables;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg;

public class SvgRenderer : IRenderer
{
	private readonly XNamespace svg = "http://www.w3.org/2000/svg";

	public string RenderDocument(SteffiDocument document)
		=> new Document(GetRenderable(document))
			.Render()
			.ToString();

	private Renderable GetRenderable(SteffiObject @object)
	{
		if (@object is Rectangle rectangle)
		{
			var canvasProperties = rectangle.ParentProperties as CanvasContainerProperties;

			var x = canvasProperties?.X ?? 0;
			var y = canvasProperties?.Y ?? 0;

			return new RectangleRenderable(x, y, rectangle.Width, rectangle.Height,
					fill: rectangle.Fill,
					fillOpacity: rectangle.FillOpacity,
					fillRule: rectangle.FillRule,
					stroke: rectangle.Stroke,
					strokeWidth: rectangle.StrokeWidth,
					strokeOpacity: rectangle.StrokeOpacity,
					strokeLineCap: rectangle.StrokeLineCap,
					rx: rectangle.Rx,
					ry: rectangle.Ry);
		}

		if (@object is Text text)
		{
			var canvasProperties = text.ParentProperties as CanvasContainerProperties;

			if (!string.IsNullOrWhiteSpace(text.Spans))
			{
				var lines = text.Spans.Split("\\n");
				var textLines = lines
					.Select(l => new TextLine(
						text: l,
						fontFamily: text.FontFamily ?? "Arial, Helvetica, sans-serif",
						fontColor: text.FontColor ?? "black",
						fontSize: text.FontSize ?? 20,
						margin: 0))
					.Cast<Renderable>()
					.ToList();

				return new VerticalStackRenderable(textLines, 0, 0, includeBorder: false) { X = canvasProperties?.X ?? 0, Y = canvasProperties?.Y ?? 0 };
			}

			return new TextLine(text.Spans ?? "", text.FontFamily ?? "Arial, Helvetica, sans-serif", text.FontColor ?? "black", text.FontSize ?? 20)
			{
				X = canvasProperties?.X ?? 0,
				Y = canvasProperties?.Y ?? 0
			};
		}


		if (@object is IParentObject parentObject)
		{
			List<Renderable> children = [];

			foreach (var child in parentObject.Children)
			{
				var childRenderable = GetRenderable(child);
				children.Add(childRenderable);
			}

			var childObject = @object as IChildObject;
			var absoluteProperties = childObject?.ParentProperties as CanvasContainerProperties;

			return parentObject switch
			{
				Canvas canvas => new CanvasContainerRenderable(children,
					padding: canvas.Padding ?? 0,
					width: canvas.Width,
					height: canvas.Height,
					includeBorder: canvas.Border ?? false,
					fill: canvas.Fill,
					stroke: canvas.Stroke,
					strokeWidth: canvas.StrokeWidth),
				HorizontalStack hStack => new HorizontalStackRenderable(children,
					padding: hStack.Padding ?? 0,
					spacing: 10,
					includeBorder: hStack.Border ?? false,
					fill: hStack.Fill,
					stroke: hStack.Stroke,
					strokeWidth: hStack.StrokeWidth)
				{ X = absoluteProperties?.X, Y = absoluteProperties?.Y },
				VerticalStack vStack => new VerticalStackRenderable(children,
					padding: vStack.Padding ?? 0,
					spacing: 10,
					includeBorder: vStack.Border ?? false,
					fill: vStack.Fill,
					stroke: vStack.Stroke,
					strokeWidth: vStack.StrokeWidth)
				{ X = absoluteProperties?.X, Y = absoluteProperties?.Y },
				SteffiDocument => new VerticalStackRenderable(children, padding: 0, spacing: 10),
				_ => throw new NotSupportedException($"Unsupported layout type: {parentObject.GetType()}")
			};
		}

		throw new InvalidOperationException($"Unknown element, no rendering for {@object.GetType().Name}");
	}
}