namespace Steffi.Models;

public record struct Point2D(int X, int Y)
{
	public static List<Point2D> ParseList(ReadOnlySpan<char> input)
	{
		var result = new List<Point2D>();

		while (input.Length > 0)
		{
			while (input.Length > 0 && input[0] == ' ')
				input = input[1..];

			if (input.Length == 0) break;

			var comma = input.IndexOf(',');
			if (comma < 0) break;

			var end = input.IndexOf(' ');
			if (end < 0) end = input.Length;

			var x = int.Parse(input[..comma]);
			var y = int.Parse(input[(comma + 1)..end]);
			result.Add(new Point2D(x, y));

			input = input[end..];
		}

		return result;
	}

	public override readonly string ToString() => $"{X},{Y}";
}
