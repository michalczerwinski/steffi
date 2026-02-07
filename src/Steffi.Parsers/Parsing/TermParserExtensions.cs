namespace Steffi.Parsers.Parsing;

public static class TermParserExtensions
{
	extension(ReadOnlySpan<char> chars)
	{
		public bool Matches(TermParser termParser) => termParser.Invoke(chars) > -1;
	}

	extension(TermParser parser)
	{
		public SyntaxRule ExactlyOne() => new SyntaxRule().Add(parser, Arity.ExactlyOne);

		public SyntaxRule OneOrMore() => new SyntaxRule().Add(parser, Arity.OneOrMore);

		public static TermParser operator |(TermParser first, TermParser second) => OneOf(first, second);

		public static TermParser operator >>(TermParser first, TermParser second)
		{
			return (input) =>
			{
				var firstMatch = first(input);

				if (firstMatch == -1)
				{
					return -1;
				}

				var secondMatch = second(input[firstMatch..]);

				if (secondMatch == -1)
				{
					return -1;
				}

				return firstMatch + secondMatch;
			};
		}

		public static TermParser String(string s) =>
			(input) => input.Length >= s.Length && input.StartsWith(s) ? s.Length : -1;

		public static TermParser Character(char c) =>
			(input) => input.Length > 0 && input[0] == c ? 1 : -1;

		public static TermParser Character(Func<char, bool> predicate) =>
			(input) => input.Length > 0 && predicate.Invoke(input[0]) ? 1 : -1;

		public static TermParser AtLeastOne(Func<char, bool> predicate) =>
			(input) =>
			{
				if (input.Length == 0 || !predicate.Invoke(input[0]))
				{
					return -1;
				}

				var pos = 1;

				while (input.Length > pos && predicate.Invoke(input[pos]))
				{
					pos++;
				}

				return pos;
			};

		public static TermParser AnythingUntil(char c)
		{
			return (input) =>
			{
				int pos = 0;

				while (pos < input.Length && input[pos] != c)
				{
					pos++;
				}

				return pos < input.Length ? pos + 1 : -1;
			};
		}

		public static TermParser AnythingUntil(string s)
		{
			return (input) =>
			{
				var index = input.IndexOf(s);

				return index >= 0 ? index + s.Length : -1;
			};
		}

		public static TermParser EverythingWhile(Func<char, bool> predicate)
		{
			return (input) =>
			{
				int pos = 0;

				while (pos < input.Length && predicate.Invoke(input[pos]))
				{
					pos++;
				}

				return pos;
			};
		}
	}


	public static TermParser OneOf(params IEnumerable<TermParser> parsers)
	{
		return (input) =>
		{
			foreach (var parser in parsers)
			{
				var result = parser(input);

				if (result > -1)
				{
					return result;
				}
			}

			return -1;
		};
	}
}
