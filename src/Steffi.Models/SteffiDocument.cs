using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class SteffiDocument : SteffiObject, IParentObject
{
	public List<SteffiObject> Children { get; } = [];

	public LayoutType Layout { get; set; }

	public ParentContainerProperties CreateContainerProperties()
	{
		return Layout switch
		{
			LayoutType.Canvas => new CanvasContainerProperties(),
			LayoutType.Vertical => new EmptyContainerProperties(),
			LayoutType.Horizontal => new EmptyContainerProperties(),
			_ => throw new InvalidOperationException($"Unsupported layout type: {Layout}")
		};
	}

	public void ResolveReferences()
	{
		var childrenByName = new Dictionary<string, SteffiObject>();

		void AddChildren(SteffiObject @object)
		{
			if (@object is INamedObject namedObject)
			{
				childrenByName[namedObject.Name] = @object;
			}

			if (@object is IParentObject parentObject)
			{
				foreach (var child in parentObject.Children)
				{
					AddChildren(child);
				}
			}
		}

		AddChildren(this);

		void ResolveReferences(SteffiObject @object)
		{
			if (@object is IParentObject parentObject)
			{
				foreach (var child in parentObject.Children)
				{
					ResolveReferences(child);
				}
			}
		}

		ResolveReferences(this);
	}
}
