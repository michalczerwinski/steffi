namespace Steffi.Parsers.Parsing;

using System.Diagnostics.CodeAnalysis;

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

    public readonly void AddError(string message) => Errors.Add($"({PositionRow}:{PositionColumn}) {message}");

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

    public readonly bool IsInputFinished() => Input.Length == 0;

    public override readonly string ToString() => $"Pos: {Position} ({PositionRow}:{PositionColumn}), Next: '{(IsInputFinished() ? "<EOF>" : Input[..Math.Min(20, Input.Length)])}'";

    public readonly ReadOnlySpan<char> GetTokenValue(ParsedToken parsedToken) => OriginalInput[parsedToken.StartAt..parsedToken.EndAt];

    public readonly ParsedToken GetNextToken()
    {
        var result = Tokens.First();
        Tokens.RemoveAt(0);

        return result;
    }

    public readonly bool PeekNextToken([NotNullWhen(true)]out ParsedToken? token, int tokenIndex = 0)
    {
        if (tokenIndex <= Tokens.Count())
        {
            token = Tokens[tokenIndex];
            return true;
        }

        token = null;
        return false;
    }

    public readonly void MoveAheadTokens(int matchLength)
    {
        for (int i = 0; i < matchLength; i++)
        {
            Tokens.RemoveAt(0);
        }
    }
}
