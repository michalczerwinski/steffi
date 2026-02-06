using SkiaSharp;

namespace Steffi.Renderers.Helpers;

internal static class TextMeasurementHelper
{
	internal static (int Width, int Height) MeasureText(string text, int fontSize, string? fontFamily = null)
	{
		using var paint = new SKPaint
		{
			TextSize = fontSize,
			IsAntialias = true,
			Typeface = string.IsNullOrWhiteSpace(fontFamily)
				? SKTypeface.Default
				: SKTypeface.FromFamilyName(fontFamily) ?? SKTypeface.Default
		};

		var width = paint.MeasureText(text);
		paint.GetFontMetrics(out var metrics);

		return (Width: (int)MathF.Ceiling(width), Height: (int)MathF.Ceiling(metrics.Descent - metrics.Ascent));
	}
}
