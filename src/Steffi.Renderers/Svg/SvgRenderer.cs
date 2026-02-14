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
		if (@object is Models.Rectangle rectangle)
		{
			var canvasProperties = rectangle.ParentProperties as CanvasContainerProperties;

			var x = canvasProperties?.X ?? 0;
			var y = canvasProperties?.Y ?? 0;

			return new Renderables.Rectangle(x, y, rectangle.Width, rectangle.Height);
		}

		if (@object is Models.Text text)
		{
			var canvasProperties = text.ParentProperties as CanvasContainerProperties;

			var x = canvasProperties?.X ?? 0;
			var y = canvasProperties?.Y ?? 0;

			return new TextLine(text.Spans ?? "", text.FontFamily ?? "Arial, Helvetica, sans-serif", text.FontColor ?? "black", text.FontSize ?? 20)
			{
				X = x,
				Y = y
			};
		}

		List<Renderable> children = [];

		var textObject = @object as Text;
		var namedObject = @object as INamedObject;

		var label = textObject?.Spans;

		if (!string.IsNullOrWhiteSpace(label))
		{
			var lines = label.Split("\\n");
			var textLines = lines
				.Select(l => new TextLine(
					text: l,
					fontFamily: textObject?.FontFamily ?? "Arial, Helvetica, sans-serif",
					fontColor: textObject?.FontColor ?? "black",
					fontSize: textObject?.FontSize ?? 20,
					margin: 0))
				.Cast<Renderable>()
				.ToList();

			var textBlock = new VerticalStackRenderable(textLines, 0, 0, includeBorder: false);
			children.Add(textBlock);
		}

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