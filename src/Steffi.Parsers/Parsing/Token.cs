namespace Steffi.Parsers.Parsing;

using System.Runtime.CompilerServices;

public record Token(Func<ReadOnlySpan<char>, TokenMatch> TryParse, [CallerMemberName] string Name = "")
{
	public override string ToString() => Name;
}

