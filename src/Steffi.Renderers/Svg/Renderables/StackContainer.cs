namespace Steffi.Renderers.Svg.Renderables;

internal abstract class StackContainer(IList<Renderable> children, int padding = 5, int spacing = 3) : Renderable
{
	protected IList<Renderable> Children { get; } = children;
	protected int Padding { get; } = padding;
	protected int Spacing { get; } = spacing;
}
