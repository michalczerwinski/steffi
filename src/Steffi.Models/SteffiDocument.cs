using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models;

public class SteffiDocument : SteffiObject, IParentObject
{
	public List<SteffiObject> Children { get; } = [];

	public ParentContainerProperties CreateContainerProperties() => new EmptyContainerProperties();

	public void ResolveReferences()
	{
		var childrenByName = new Dictionary<string, SteffiObject>();

		void AddChildren(SteffiObject @object)
		{
			if (!string.IsNullOrEmpty(@object.Name))
			{
				childrenByName[@object.Name] = @object;
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
