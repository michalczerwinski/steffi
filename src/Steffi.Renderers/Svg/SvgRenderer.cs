using Steffi.Models;
using Steffi.Models.Interfaces;
using Steffi.Renderers.Svg.Renderables;
using System.Xml.Linq;

namespace Steffi.Renderers.Svg;

public class SvgRenderer : IRenderer
{
	private readonly XNamespace svg = "http://www.w3.org/2000/svg";

	public string RenderDocument(SteffiDocument document)
	{
		var renderable = GetRenderable(document).Render(0, 0);

		return new Document(GetRenderable(document))
			.Render()
			.ToString();
	}

	private Renderable GetRenderable(SteffiObject @object)
	{
		List<Renderable> children = [];

		if (@object is INamedObject namedObject)
		{
			children.Add(new TextLine(namedObject.Name));
		}

		if (@object is IParentObject parentObject)
		{
			foreach (var child in parentObject.Children)
			{
				var childRenderable = GetRenderable(child);
				children.Add(childRenderable);
			}
		}

		return new VerticalStackContainer(children);
	}
}