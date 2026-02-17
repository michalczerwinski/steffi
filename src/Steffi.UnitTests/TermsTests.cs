using Steffi.Parsers;

namespace Steffi.UnitTests;

public class TermsTests
{
	// WhiteSpaceBlock

	[Test, DisplayName("WhiteSpaceBlock matches spaces")]
	public async Task WhiteSpaceBlockMatchesSpaces()
		=> await Assert.That(Terms.WhiteSpaceBlock("   abc")).IsEqualTo(3);

	[Test, DisplayName("WhiteSpaceBlock matches tabs and newlines")]
	public async Task WhiteSpaceBlockMatchesTabsAndNewlines()
		=> await Assert.That(Terms.WhiteSpaceBlock("\t\n\r ")).IsEqualTo(4);

	[Test, DisplayName("WhiteSpaceBlock fails on non-whitespace")]
	public async Task WhiteSpaceBlockFailsOnNonWhitespace()
		=> await Assert.That(Terms.WhiteSpaceBlock("abc")).IsEqualTo(-1);

	[Test, DisplayName("WhiteSpaceBlock fails on empty input")]
	public async Task WhiteSpaceBlockFailsOnEmpty()
		=> await Assert.That(Terms.WhiteSpaceBlock("")).IsEqualTo(-1);

	// LineComment

	[Test, DisplayName("LineComment matches until newline")]
	public async Task LineCommentMatchesUntilNewline()
		=> await Assert.That(Terms.LineComment("// a comment\nnext")).IsEqualTo(13);

	[Test, DisplayName("LineComment fails without double slash prefix")]
	public async Task LineCommentFailsWithoutPrefix()
		=> await Assert.That(Terms.LineComment("not a comment")).IsEqualTo(-1);

	[Test, DisplayName("LineComment fails with single slash")]
	public async Task LineCommentFailsWithSingleSlash()
		=> await Assert.That(Terms.LineComment("/ not a comment\n")).IsEqualTo(-1);

	// Identifier

	[Test, DisplayName("Identifier matches simple name")]
	public async Task IdentifierMatchesSimpleName()
		=> await Assert.That(Terms.Identifier("canvas ")).IsEqualTo(6);

	[Test, DisplayName("Identifier matches name with digits")]
	public async Task IdentifierMatchesNameWithDigits()
		=> await Assert.That(Terms.Identifier("item2{")).IsEqualTo(5);

	[Test, DisplayName("Identifier matches underscore prefix")]
	public async Task IdentifierMatchesUnderscorePrefix()
		=> await Assert.That(Terms.Identifier("_name;")).IsEqualTo(5);

	[Test, DisplayName("Identifier matches name with underscores")]
	public async Task IdentifierMatchesNameWithUnderscores()
		=> await Assert.That(Terms.Identifier("my_item_1 ")).IsEqualTo(9);

	[Test, DisplayName("Identifier fails when starting with digit")]
	public async Task IdentifierFailsStartingWithDigit()
		=> await Assert.That(Terms.Identifier("1abc")).IsEqualTo(-1);

	[Test, DisplayName("Identifier fails on empty input")]
	public async Task IdentifierFailsOnEmpty()
		=> await Assert.That(Terms.Identifier("")).IsEqualTo(-1);

	// AssignmentEnd

	[Test, DisplayName("AssignmentEnd matches semicolon")]
	public async Task AssignmentEndMatchesSemicolon()
		=> await Assert.That(Terms.AssignmentEnd(";")).IsEqualTo(1);

	[Test, DisplayName("AssignmentEnd fails on non-semicolon")]
	public async Task AssignmentEndFailsOnNonSemicolon()
		=> await Assert.That(Terms.AssignmentEnd(":")).IsEqualTo(-1);

	[Test, DisplayName("AssignmentEnd fails on empty input")]
	public async Task AssignmentEndFailsOnEmpty()
		=> await Assert.That(Terms.AssignmentEnd("")).IsEqualTo(-1);

	// IntegerNumber

	[Test, DisplayName("IntegerNumber matches digits")]
	public async Task IntegerNumberMatchesDigits()
		=> await Assert.That(Terms.IntegerNumber("123;")).IsEqualTo(3);

	[Test, DisplayName("IntegerNumber matches single digit")]
	public async Task IntegerNumberMatchesSingleDigit()
		=> await Assert.That(Terms.IntegerNumber("0")).IsEqualTo(1);

	[Test, DisplayName("IntegerNumber stops at non-digit")]
	public async Task IntegerNumberStopsAtNonDigit()
		=> await Assert.That(Terms.IntegerNumber("42px")).IsEqualTo(2);

	[Test, DisplayName("IntegerNumber fails on non-digit")]
	public async Task IntegerNumberFailsOnNonDigit()
		=> await Assert.That(Terms.IntegerNumber("abc")).IsEqualTo(-1);

	// FloatingNumber

	[Test, DisplayName("FloatingNumber matches decimal value")]
	public async Task FloatingNumberMatchesDecimal()
		=> await Assert.That(Terms.FloatingNumber("3.14;")).IsEqualTo(4);

	[Test, DisplayName("FloatingNumber matches single digit parts")]
	public async Task FloatingNumberMatchesSingleDigitParts()
		=> await Assert.That(Terms.FloatingNumber("0.5 ")).IsEqualTo(3);

	[Test, DisplayName("FloatingNumber fails without decimal point")]
	public async Task FloatingNumberFailsWithoutDecimalPoint()
		=> await Assert.That(Terms.FloatingNumber("123")).IsEqualTo(-1);

	[Test, DisplayName("FloatingNumber fails with trailing dot")]
	public async Task FloatingNumberFailsWithTrailingDot()
		=> await Assert.That(Terms.FloatingNumber("3.")).IsEqualTo(-1);

	// BlockComment

	[Test, DisplayName("BlockComment matches complete block")]
	public async Task BlockCommentMatchesCompleteBlock()
		=> await Assert.That(Terms.BlockComment("/* comment */next")).IsEqualTo(13);

	[Test, DisplayName("BlockComment matches multiline")]
	public async Task BlockCommentMatchesMultiline()
		=> await Assert.That(Terms.BlockComment("/* line1\nline2 */")).IsEqualTo(17);

	[Test, DisplayName("BlockComment matches empty comment")]
	public async Task BlockCommentMatchesEmpty()
		=> await Assert.That(Terms.BlockComment("/**/")).IsEqualTo(4);

	[Test, DisplayName("BlockComment fails without closing")]
	public async Task BlockCommentFailsWithoutClosing()
		=> await Assert.That(Terms.BlockComment("/* unclosed")).IsEqualTo(-1);

	[Test, DisplayName("BlockComment fails on non-comment")]
	public async Task BlockCommentFailsOnNonComment()
		=> await Assert.That(Terms.BlockComment("not a comment")).IsEqualTo(-1);

	// NestingOpen

	[Test, DisplayName("NestingOpen matches opening brace")]
	public async Task NestingOpenMatchesBrace()
		=> await Assert.That(Terms.NestingOpen("{")).IsEqualTo(1);

	[Test, DisplayName("NestingOpen fails on closing brace")]
	public async Task NestingOpenFailsOnClosingBrace()
		=> await Assert.That(Terms.NestingOpen("}")).IsEqualTo(-1);

	// NestingClose

	[Test, DisplayName("NestingClose matches closing brace")]
	public async Task NestingCloseMatchesBrace()
		=> await Assert.That(Terms.NestingClose("}")).IsEqualTo(1);

	[Test, DisplayName("NestingClose fails on opening brace")]
	public async Task NestingCloseFailsOnOpeningBrace()
		=> await Assert.That(Terms.NestingClose("{")).IsEqualTo(-1);

	// PropertySeparator

	[Test, DisplayName("PropertySeparator matches colon")]
	public async Task PropertySeparatorMatchesColon()
		=> await Assert.That(Terms.PropertySeparator(":")).IsEqualTo(1);

	[Test, DisplayName("PropertySeparator fails on semicolon")]
	public async Task PropertySeparatorFailsOnSemicolon()
		=> await Assert.That(Terms.PropertySeparator(";")).IsEqualTo(-1);

	// StringLiteral

	[Test, DisplayName("StringLiteral matches quoted string")]
	public async Task StringLiteralMatchesQuotedString()
		=> await Assert.That(Terms.StringLiteral("\"hello\";")).IsEqualTo(7);

	[Test, DisplayName("StringLiteral matches empty string")]
	public async Task StringLiteralMatchesEmptyString()
		=> await Assert.That(Terms.StringLiteral("\"\"")).IsEqualTo(2);

	[Test, DisplayName("StringLiteral matches string with spaces")]
	public async Task StringLiteralMatchesStringWithSpaces()
		=> await Assert.That(Terms.StringLiteral("\"hello world\"")).IsEqualTo(13);

	[Test, DisplayName("StringLiteral fails without opening quote")]
	public async Task StringLiteralFailsWithoutOpeningQuote()
		=> await Assert.That(Terms.StringLiteral("hello\"")).IsEqualTo(-1);

	[Test, DisplayName("StringLiteral fails without closing quote")]
	public async Task StringLiteralFailsWithoutClosingQuote()
		=> await Assert.That(Terms.StringLiteral("\"unclosed")).IsEqualTo(-1);

	// PointsList

	[Test, DisplayName("PointsList matches single point")]
	public async Task PointsListMatchesSinglePoint()
		=> await Assert.That(Terms.PointsList("50,80;")).IsEqualTo(5);

	[Test, DisplayName("PointsList matches multiple points")]
	public async Task PointsListMatchesMultiplePoints()
		=> await Assert.That(Terms.PointsList("50,0 100,80 0,80;")).IsEqualTo(16);

	[Test, DisplayName("PointsList matches decimal points")]
	public async Task PointsListMatchesDecimalPoints()
		=> await Assert.That(Terms.PointsList("1.5,2.5 3.0,4.0;")).IsEqualTo(15);

	[Test, DisplayName("PointsList matches mixed integer and decimal points")]
	public async Task PointsListMatchesMixedPoints()
		=> await Assert.That(Terms.PointsList("10,20.5 30.5,40;")).IsEqualTo(15);

	[Test, DisplayName("PointsList stops before semicolon")]
	public async Task PointsListStopsBeforeSemicolon()
	{
		var input = "50,0 100,80;";
		var consumed = Terms.PointsList(input);
		await Assert.That(input.AsSpan()[..consumed].ToString()).IsEqualTo("50,0 100,80");
	}

	[Test, DisplayName("PointsList fails on plain integer")]
	public async Task PointsListFailsOnPlainInteger()
		=> await Assert.That(Terms.PointsList("100;")).IsEqualTo(-1);

	[Test, DisplayName("PointsList fails on empty input")]
	public async Task PointsListFailsOnEmpty()
		=> await Assert.That(Terms.PointsList("")).IsEqualTo(-1);
}
