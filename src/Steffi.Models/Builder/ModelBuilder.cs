namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	public static bool SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
		=> SetParentProperty(steffiObject, propertyName, value)
		|| SetRectangleProperty(steffiObject, propertyName, value)
		|| SetCircleProperty(steffiObject, propertyName, value)
		|| SetEllipseProperty(steffiObject, propertyName, value)
		|| SetTextProperty(steffiObject, propertyName, value)
		|| SetCanvasProperty(steffiObject, propertyName, value)
		|| SetHorizontalStackProperty(steffiObject, propertyName, value)
		|| SetVerticalStackProperty(steffiObject, propertyName, value);
}
