namespace Steffi.Parsers.Parsing;

public ref struct ParsingContext
{
	public ReadOnlySpan<char> OriginalInput { get; private set; }

	public ReadOnlySpan<char> Input { get; private set; }

	public ParsingContext(ReadOnlySpan<char> input)
	{
		Input = input;
		OriginalInput = input;
	}

	public int Position { get; private set; } = 0;

	public int PositionRow { get; private set; } = 1;

	public int PositionColumn { get; private set; } = 1;

	public List<ParsedToken> Tokens { get; } = [];

	public List<string> Errors { get; } = [];

	public void AddError(string message) => Errors.Add($"({PositionRow}:{PositionColumn}) {message}");

	public void MoveAheadInput(int length)
	{
		for (int i = 0; i < length; i++)
		{
			if (Input[i] == '\n')
			{
				PositionRow++;
				PositionColumn = 1;
			}
			else
			{
				PositionColumn++;
			}
		}
		Input = Input[length..];
		Position += length;
	}

	public bool IsInputFinished() => Input.Length == 0;

	public override string ToString() => $"Pos: {Position} ({PositionRow}:{PositionColumn}), Next: '{(IsInputFinished() ? "<EOF>" : Input[..Math.Min(20, Input.Length)])}'";

	public ReadOnlySpan<char> GetTokenValue(ParsedToken parsedToken) => OriginalInput[parsedToken.StartAt..parsedToken.EndAt];

	public ParsedToken GetNextToken()
	{
		var result = Tokens.First();
		Tokens.RemoveAt(0);

		return result;
	}

	public ParsedToken PeekNextToken(int tokenIndex = 0) => Tokens[tokenIndex];

	public void MoveAheadTokens(int matchLength)
	{
		for (int i = 0; i < matchLength; i++)
		{
			Tokens.RemoveAt(0);
		}
	}
}

