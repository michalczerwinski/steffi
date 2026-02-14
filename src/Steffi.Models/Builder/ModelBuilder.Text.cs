namespace Steffi.Models.Builder;

public static partial class ModelBuilder
{
	private static bool SetTextProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is Text text)
		{
			if (propertyName.Equals(nameof(Text.Spans), StringComparison.InvariantCultureIgnoreCase))
			{
				text.Spans = value.ToString();
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontColor), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontColor = value.ToString();
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontSize), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontSize = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontFamily), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontFamily = value.ToString();
				return true;
			}
		}

		return false;
	}
}