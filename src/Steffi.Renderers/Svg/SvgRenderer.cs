using Steffi.Models;
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
		List<Renderable> children = [];

		var labeledObject = @object as ILabeledObject;
		var namedObject = @object as INamedObject;

		var label = labeledObject?.Label ?? namedObject?.Name;

		if (!string.IsNullOrWhiteSpace(label))
		{
			children.Add(new TextLine(label, labeledObject?.FontColor ?? "black"));
		}

		if (@object is IParentObject parentObject)
		{
			foreach (var child in parentObject.Children)
			{
				var childRenderable = GetRenderable(child);
				children.Add(childRenderable);
			}

			return parentObject.Layout switch
			{
				LayoutType.Horizontal => new HorizontalStackContainer(children),
				LayoutType.Vertical => new VerticalStackContainer(children),
				_ => throw new NotSupportedException($"Unsupported layout type: {parentObject.Layout}")
			};
		}

		return new VerticalStackContainer(children);
	}
}