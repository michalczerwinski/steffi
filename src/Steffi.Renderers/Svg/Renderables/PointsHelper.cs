using Steffi.Models;

namespace Steffi.Renderers.Svg.Renderables;

internal static class PointsHelper
{
	internal static (decimal Width, decimal Height) MeasureBounds(List<Point2D> points)
	{
		decimal maxX = 0;
		decimal maxY = 0;

		foreach (var p in points)
		{
			if (p.X > maxX) maxX = p.X;
			if (p.Y > maxY) maxY = p.Y;
		}

		return (maxX, maxY);
	}

	internal static string ToSvgString(List<Point2D> points, decimal dx, decimal dy)
	{
		if (dx == 0 && dy == 0)
			return string.Join(' ', points);

		return string.Join(' ', points.Select(p => $"{p.X + dx},{p.Y + dy}"));
	}
}
