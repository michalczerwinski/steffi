using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	private static bool SetParentProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is IChildObject childObject && childObject.ParentProperties is CanvasContainerProperties canvasContainerProperties)
		{
			if (propertyName.Equals(nameof(CanvasContainerProperties.X), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.X = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(CanvasContainerProperties.Y), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.Y = int.Parse(value);
				return true;
			}
		}

		return false;
	}
}