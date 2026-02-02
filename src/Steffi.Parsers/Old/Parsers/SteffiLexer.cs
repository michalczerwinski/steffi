using Steffi.Parsers.Old.Parsing;
using Steffi.Parsers.Old.Parsing.Lexer;

namespace Steffi.Parsers.Old.Parsers;

public class SteffiLexer : LexerBase
{
	public readonly static Token LineComment = new((input) =>
	{
		if (!input.StartsWith("//"))
		{
			return new(false);
		}

		var endOfLineIndex = input.IndexOf('\n');

		return endOfLineIndex == -1
			? new(true, input.Length)
			: new(true, endOfLineIndex + 1);
	});

	public readonly static Token Identifier = new((input) =>
	{
		if (input.Length == 0 || (!char.IsLetter(input[0]) && input[0] != '_'))
		{
			return new(false);
		}

		int length = 1;
		while (length < input.Length && (char.IsLetterOrDigit(input[length]) || input[length] == '_'))
		{
			length++;
		}

		return new(true, length);
	});

	public readonly static Token IntegerNumber = new((input) =>
	{
		if (input.Length == 0 || !char.IsDigit(input[0]))
		{
			return new(false);
		}

		int length = 1;

		while (length < input.Length && char.IsDigit(input[length]))
		{
			length++;
		}

		return new(true, length);
	});

	public readonly static Token BlockComment = new((input) =>
	{
		if (!input.StartsWith("/*"))
		{
			return new(false);
		}

		var endIndex = input.Slice(2).IndexOf("*/");

		if (endIndex == -1)
		{
			return new(false);
		}

		return new(true, endIndex + 2 + 2);
	});

	public readonly static Token WhiteSpace = new((input) =>
	{
		int length = 0;
		while (length < input.Length && char.IsWhiteSpace(input[length]))
		{
			length++;
		}

		return length > 0
			? new(true, length)
			: new(false);
	});

	public readonly static Token NestingOpen = new((input) => input.Length > 0 && input[0] == '{' ? new(true, 1) : new(false));

	public readonly static Token NestingClose = new((input) => input.Length > 0 && input[0] == '}' ? new(true, 1) : new(false));

	public readonly static Token PropertySeparator = new((input) => input.Length > 0 && input[0] == ':' ? new(true, 1) : new(false));

	public readonly static Token PropertyEnd = new((input) => input.Length > 0 && input[0] == ';' ? new(true, 1) : new(false));

	public override Token[] KnownTokens => [LineComment, BlockComment, Identifier, IntegerNumber, WhiteSpace, NestingOpen, NestingClose, PropertySeparator, PropertyEnd];

	public override bool ShouldBeIgnored(Token token) =>
		token == WhiteSpace
		|| token == LineComment
		|| token == BlockComment;
}
