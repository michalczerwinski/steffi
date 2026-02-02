using Steffi.Parsers.Parsing;

namespace Steffi.Parsers;

public static class Syntax
{
	public static SyntaxRule ObjectDeclaration = new SyntaxRule()
		.Add(Terms.WhiteSpace | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("TypeIdentifier", Terms.Identifier, Arity.ExactlyOne)
		.Add(Terms.WhiteSpace | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("OptionalName", Terms.Identifier, Arity.ZeroOrOne)
		.Add(Terms.WhiteSpace | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add(Terms.NestingOpen, Arity.ExactlyOne);

	public static SyntaxRule PropertyAssignment = new SyntaxRule()
		.Add(Terms.WhiteSpace | Terms.LineComment | Terms.BlockComment, Arity.ZeroOrMore)
		.Add("PropertyName", Terms.Identifier, Arity.ExactlyOne)
		.Add(Terms.PropertySeparator, Arity.ExactlyOne)
		.Add("PropertyValue", Terms.Identifier | Terms.IntegerNumber | Terms.WhiteSpace, Arity.OneOrMore)
		.Add(Terms.AssignmentEnd, Arity.ExactlyOne);
}