using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public static class Syntax
{
	public readonly static SyntaxRule ObjectDeclaration = new SyntaxRule()
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("TypeIdentifier", Terms.Identifier, Arity.ExactlyOne)
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("OptionalName", Terms.Identifier, Arity.ZeroOrOne)
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add(Terms.NestingOpen, Arity.ExactlyOne);

	public readonly static SyntaxRule PropertyAssignment = new SyntaxRule()
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("PropertyName", Terms.Identifier, Arity.ExactlyOne)
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add(Terms.PropertySeparator, Arity.ExactlyOne)
		.Add(Terms.WhiteSpaceBlock | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("PropertyValue", Terms.FloatingNumber | Terms.IntegerNumber | Terms.StringLiteral | Terms.Identifier, Arity.ExactlyOne)
		.Add(Terms.AssignmentEnd, Arity.ExactlyOne);
}