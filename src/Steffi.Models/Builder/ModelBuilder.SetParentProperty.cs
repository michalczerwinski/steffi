using Steffi.Models.Containers.Properties;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	private static bool SetParentProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is IChildObject childObject && childObject.ParentProperties is CanvasParentProperties canvasContainerProperties)
		{
			if (propertyName.Equals(nameof(CanvasParentProperties.X), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.X = decimal.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(CanvasParentProperties.Y), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.Y = decimal.Parse(value);
				return true;
			}
		}

		return false;
	}
}