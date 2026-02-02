namespace Steffi.Parsers.Parsing;

public class SyntaxRule()
{
	internal List<(string? Name, TermParser, Arity Arity)> Segments { get; private set; } = [];

	public SyntaxRule Add(TermParser termParser, Arity arity) => Add(null, termParser, arity);

	public SyntaxRule Add(string? name, TermParser termParser, Arity arity)
	{
		Segments.Add((name, termParser, arity));

		return this;
	}

	internal static (bool matched, int matchedLength) MatchSegment(ReadOnlySpan<char> input, TermParser termParser, Arity arity)
	{
		int segmentMatched;

		switch (arity)
		{
			case Arity.ExactlyOne:
				segmentMatched = termParser(input);
				return segmentMatched > 0 ? (true, segmentMatched) : (false, 0);

			case Arity.ZeroOrOne:
				segmentMatched = termParser(input);
				return (true, segmentMatched);

			case Arity.ZeroOrMore:
				segmentMatched = 0;

				while (true)
				{
					var matched = termParser(input);

					if (matched == 0)
					{
						break;
					}

					input = input[matched..];
					segmentMatched += matched;
				}

				return (true, segmentMatched);

			case Arity.OneOrMore:
				var firstMatch = termParser(input);
				if (firstMatch == 0)
				{
					return (false, 0);
				}
				segmentMatched = firstMatch;
				input = input[firstMatch..];
				while (true)
				{
					var matched = termParser(input);
					if (matched == 0)
					{
						break;
					}
					segmentMatched += matched;
					input = input[matched..];
				}

				return (true, segmentMatched);
		}

		return (false, 0);
	}
}

