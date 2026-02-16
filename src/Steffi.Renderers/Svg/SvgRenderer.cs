using Steffi.Models;
using Steffi.Models.Containers;
using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;
using Steffi.Models.Properties;
using Steffi.Renderers.Svg.Renderables;
using Steffi.Renderers.Svg.Renderables.Containers;

namespace Steffi.Renderers.Svg;

public class SvgRenderer : IRenderer
{
	public string RenderDocument(SteffiDocument document)
		=> new Document(GetRenderable(document))
			.Render()
			.ToString();

	private Renderable GetRenderable(SteffiObject @object) => @object switch
	{
		Rectangle rectangle => GetRenderableForRectangle(rectangle),
		Text text => GetRenderableForText(text),
		IParentObject parentObject => GetRenderableForParentObject(parentObject),
		_ => throw new NotSupportedException($"Unsupported element type: {@object.GetType()}"),
	};

	private Renderable GetRenderableForParentObject(IParentObject parentObject)
	{
		List<Renderable> children = [.. parentObject.Children.Select(GetRenderable)];
		var childObject = parentObject as IChildObject;
		var absoluteProperties = childObject?.ParentProperties as CanvasParentProperties;

		return parentObject switch
		{
			Canvas canvas => new CanvasRenderable(children,
				fillAndStroke: canvas,
				padding: canvas.Padding ?? 0,
				width: canvas.Width,
				height: canvas.Height,
				includeBorder: canvas.Border ?? false)
			{ X = absoluteProperties?.X, Y = absoluteProperties?.Y },
			HorizontalStack hStack => new HorizontalStackRenderable(children,
				fillAndStroke: hStack,
				padding: hStack.Padding ?? 0,
				spacing: hStack.Spacing ?? 10,
				includeBorder: hStack.Border ?? false)
			{ X = absoluteProperties?.X, Y = absoluteProperties?.Y },
			VerticalStack vStack => new VerticalStackRenderable(children,
				fillAndStroke: vStack,
				padding: vStack.Padding ?? 0,
				spacing: vStack.Spacing ?? 10,
				includeBorder: vStack.Border ?? false)
			{ X = absoluteProperties?.X, Y = absoluteProperties?.Y },
			SteffiDocument steffiDocument => children.Single(),
			_ => throw new NotSupportedException($"Unsupported layout type: {parentObject.GetType()}")
		};

		throw new InvalidOperationException($"Unknown element, no rendering for {parentObject.GetType().Name}");
	}

	private Renderable GetRenderableForText(Text text)
	{
		var canvasProperties = text.ParentProperties as CanvasParentProperties;

		var lines = text.Spans?.Split("\\n") ?? [];
		var textLines = lines
			.Select(l => new TextLine(
				text: l,
				fontFamily: text.FontFamily ?? "Arial, Helvetica, sans-serif",
				fontColor: text.FontColor ?? "black",
				fontSize: text.FontSize ?? 20,
				margin: 0))
			.Cast<Renderable>()
			.ToList();

		return new VerticalStackRenderable(textLines, new FillAndStrokeProperties { Stroke = "none", Fill = "white" }, padding: 0, spacing: 0, includeBorder: false) { X = canvasProperties?.X ?? 0, Y = canvasProperties?.Y ?? 0 };
	}

	private Renderable GetRenderableForRectangle(Rectangle rectangle)
	{
		var canvasProperties = rectangle.ParentProperties as CanvasParentProperties;

		return new RectangleRenderable(
				x: canvasProperties?.X ?? 0,
				y: canvasProperties?.Y ?? 0,
				width: rectangle.Width,
				height: rectangle.Height,
				fillAndStroke: rectangle,
				rx: rectangle.Rx,
				ry: rectangle.Ry);
	}
}