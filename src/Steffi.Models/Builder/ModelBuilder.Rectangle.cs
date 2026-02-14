namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	private static bool SetRectangleProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is Rectangle rectangle)
		{
			if (propertyName.Equals(nameof(Rectangle.Width), StringComparison.InvariantCultureIgnoreCase))
			{
				rectangle.Width = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(Rectangle.Height), StringComparison.InvariantCultureIgnoreCase))
			{
				rectangle.Height = int.Parse(value);
				return true;
			}
		}

		return false;
	}
}