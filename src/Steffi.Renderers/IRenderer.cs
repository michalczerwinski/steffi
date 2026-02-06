using Steffi.Models;

namespace Steffi.Renderers;

public interface IRenderer
{
	public string RenderDocument(SteffiDocument document);
}
