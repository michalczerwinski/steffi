using Steffi.Models.Containers;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	public static SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name, IParentObject parentObject) => tokenType switch
	{
		nameof(Canvas) => new Canvas { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(HorizontalStack) => new HorizontalStack { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(VerticalStack) => new VerticalStack { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(Rectangle) => new Rectangle { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(Text) => new Text { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		_ => null,
	};

	public static bool SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
		=> SetTextProperty(steffiObject, propertyName, value)
			|| SetIChildObjectProperty(steffiObject, propertyName, value)
			|| SetRectangleProperty(steffiObject, propertyName, value);
}
