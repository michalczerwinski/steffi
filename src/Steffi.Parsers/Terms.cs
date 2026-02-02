using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public static class Terms
{
	public readonly static TermParser WhiteSpaceBlock = TermParser.AtLeastOne(char.IsWhiteSpace);

	public readonly static TermParser LineComment = TermParser.String("//") >> TermParser.AnythingUntil('\n');

	public readonly static TermParser Identifier =
		TermParser.Character(c => char.IsLetter(c) || c == '_')
		>> TermParser.EverythingWhile(char.IsLetterOrDigit);

	public readonly static TermParser AssignmentEnd = TermParser.Character(';');

	public readonly static TermParser IntegerNumber = TermParser.AtLeastOne(char.IsDigit);

	public readonly static TermParser BlockComment = TermParser.String("/*") >> TermParser.AnythingUntil("*/");

	public readonly static TermParser NestingOpen = TermParser.Character('{');

	public readonly static TermParser NestingClose = TermParser.Character('}');

	public readonly static TermParser PropertySeparator = TermParser.Character(':');
}