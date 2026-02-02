using System.Diagnostics.CodeAnalysis;

namespace Steffi.Parsers.Parsing;

public ref struct ParsingContext(ReadOnlySpan<char> input, ReadOnlySpan<char> remaining, (int Index, int Row, int Column) position)
{
	public ParsingContext(ReadOnlySpan<char> input) : this(input, input, (0, 1, 1))
	{
	}

	public ReadOnlySpan<char> Input { get; } = input;
	public ReadOnlySpan<char> Remaining { get; private set; } = remaining;

	public (int Index, int Row, int Column) Position { get; private set; } = position;

	public bool EndReached => Remaining.Length == 0;

	public bool Match(SyntaxRule syntaxRule, [NotNullWhen(true)] out MatchedSegment[]? matchedSegments)
	{
		var contextBefore = this;
		matchedSegments = new MatchedSegment[syntaxRule.Segments.Count(s => s.Name is not null)];
		int matchedSegmentIndex = 0;

		foreach (var (name, termParser, arity) in syntaxRule.Segments)
		{
			var (matched, matchedLength) = syntaxRule.MatchSegment(Remaining, termParser, arity);

			if (!matched)
			{
				matchedSegments = null;
				this = contextBefore;
				return false;
			}

			if (matchedLength > 0)
			{
				var positionBefore = Position;
				Advance(matchedLength);
				if (name is not null)
				{
					matchedSegments[matchedSegmentIndex++] = new(name, positionBefore.Index, matchedLength, positionBefore.Row, positionBefore.Column);
				}

			}
		}

		return true;
	}

	public void Advance(int length)
	{
		var newRemaining = Remaining.Slice(length);
		int newIndex = Position.Index + length;
		int newRow = Position.Row;
		int newColumn = Position.Column;

		for (int i = 0; i < length; i++)
		{
			if (Remaining[i] == '\n')
			{
				newRow++;
				newColumn = 1;
			}
			else
			{
				newColumn++;
			}
		}

		Position = (newIndex, newRow, newColumn);
		Remaining = newRemaining;
	}

	public string GetPositionString() => $"({Position.Row},{Position.Column}):";
}
