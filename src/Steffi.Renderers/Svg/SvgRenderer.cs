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

			return new RectangleRenderable(x, y, rectangle.Width, rectangle.Height, rx: rectangle.Rx, ry: rectangle.Ry);
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


		List<Renderable> children = [];

		if (@object is IParentObject parentObject)
		{

			foreach (var child in parentObject.Children)
			{
				var childRenderable = GetRenderable(child);
				children.Add(childRenderable);
			}

			var childObject = @object as IChildObject;
			var absoluteProperties = childObject?.ParentProperties as CanvasContainerProperties;

			return parentObject switch
			{
				Canvas => new CanvasContainer(children, padding: 5),
				HorizontalStack => new HorizontalStackRenderable(children, padding: 5, spacing: 10) { X = absoluteProperties?.X, Y = absoluteProperties?.Y },
				VerticalStack => new VerticalStackRenderable(children, padding: 5, spacing: 10) { X = absoluteProperties?.X, Y = absoluteProperties?.Y },
				SteffiDocument => new VerticalStackRenderable(children, padding: 5, spacing: 10),
				_ => throw new NotSupportedException($"Unsupported layout type: {parentObject.GetType()}")
			};
		}

		return new VerticalStackRenderable(children);
	}
}