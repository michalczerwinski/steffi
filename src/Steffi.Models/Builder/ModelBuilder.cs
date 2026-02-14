using Steffi.Models.Containers;
using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{

	public static bool SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
		=> SetTextProperty(steffiObject, propertyName, value)
			|| SetIChildObjectProperty(steffiObject, propertyName, value)
			|| SetRectangleProperty(steffiObject, propertyName, value);
}
