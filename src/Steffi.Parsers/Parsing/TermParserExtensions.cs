namespace Steffi.Parsers.Parsing;

public static class TermParserExtensions
{
	extension(TermParser parser)
	{
		public static TermParser operator |(TermParser first, TermParser second) => OneOf(first, second);

		public SyntaxRule ExactlyOne() => new SyntaxRule().Add(parser, Arity.ExactlyOne);

		public SyntaxRule OneOrMore() => new SyntaxRule().Add(parser, Arity.OneOrMore);
	}


	public static TermParser OneOf(params IEnumerable<TermParser> parsers)
	{
		return (input) =>
		{
			foreach (var parser in parsers)
			{
				var result = parser(input);

				if (result > 0)
				{
					return result;
				}
			}

			return 0;
		};
	}
}
