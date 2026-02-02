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

	public bool Matches(SyntaxRule syntaxRule) => Matches(syntaxRule, out var _, out _, out _);

	public bool Matches(SyntaxRule syntaxRule, out MatchedSegment segment1) => Matches(syntaxRule, out segment1, out _, out _);

	public bool Matches(SyntaxRule syntaxRule, out MatchedSegment segment1, out MatchedSegment segment2) => Matches(syntaxRule, out segment1, out segment2, out _);

	public bool Matches(SyntaxRule syntaxRule, out MatchedSegment segment1, out MatchedSegment segment2, out MatchedSegment segment3)
	{
		segment1 = default;
		segment2 = default;
		segment3 = default;

		var contextBefore = this;
		int matchedSegmentIndex = 0;

		foreach (var (name, termParser, arity) in syntaxRule.Segments)
		{
			var (matched, matchedLength) = SyntaxRule.MatchSegment(Remaining, termParser, arity);

			if (!matched)
			{
				this = contextBefore;
				return false;
			}

			if (matchedLength > 0)
			{
				var positionBefore = Position;
				var inputBefore = Remaining;

				Advance(matchedLength);
				if (name is not null)
				{
					var matchedSegment = new MatchedSegment(name, positionBefore.Index, matchedLength, positionBefore.Row, positionBefore.Column, inputBefore.Slice(0, matchedLength));
					switch (matchedSegmentIndex)
					{
						case 0:
							segment1 = matchedSegment;
							break;
						case 1:
							segment2 = matchedSegment;
							break;
						case 2:
							segment3 = matchedSegment;
							break;
					}
					matchedSegmentIndex++;
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

	public string CreateError(ReadOnlySpan<char> error) => $"{GetPositionString()} {error}";

}
