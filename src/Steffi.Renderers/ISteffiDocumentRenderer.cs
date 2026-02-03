using Steffi.Models;

namespace Steffi.Renderers;

public interface ISteffiDocumentRenderer
{
	public string RenderDocument(SteffiDocument document);
}
