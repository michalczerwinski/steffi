using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public static class Terms
{
	public static TermParser WhiteSpace = (input) =>
	{
		int pos = 0;

		while (input.Length > pos && char.IsWhiteSpace(input[pos]))
		{
			pos++;
		}

		return pos;
	};

	public static TermParser LineComment = (input) =>
	{
		if (input.StartsWith("//"))
		{
			int pos = 2;

			while (input.Length > pos && input[pos] != '\n')
			{
				pos++;
			}

			return pos;
		}

		return 0;
	};

	public static TermParser Identifier = (input) =>
	{
		int pos = 0;

		if (input.Length > pos && (char.IsLetter(input[pos]) || input[pos] == '_'))
		{
			pos++;

			while (input.Length > pos && (char.IsLetterOrDigit(input[pos]) || input[pos] == '_'))
			{
				pos++;
			}
		}

		return pos;
	};

	public static TermParser AssignmentEnd = (input) => input.Length > 0 && input[0] == ';' ? 1 : 0;

	public static TermParser IntegerNumber = (input) =>
	{
		int pos = 0;

		while (input.Length > 0 && char.IsDigit(input[pos]))
		{
			pos++;
		}

		return pos;
	};

	public static TermParser BlockComment = (input) =>
	{
		if (input.StartsWith("/*"))
		{
			var index = input[2..].IndexOf("*/");

			return index != -1 ? index + 2 + 2 : 0;
		}

		return 0;
	};

	public static TermParser NestingOpen = (input) => input.Length > 0 && input[0] == '{' ? 1 : 0;

	public static TermParser NestingClose = (input) => input.Length > 0 && input[0] == '}' ? 1 : 0;

	public static TermParser PropertySeparator = (input) => input.Length > 0 && input[0] == ':' ? 1 : 0;
}