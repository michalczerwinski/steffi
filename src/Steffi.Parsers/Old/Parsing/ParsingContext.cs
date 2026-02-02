namespace Steffi.Parsers.Old.Parsing;

using Steffi.Parsers.Old.Parsing.Lexer;
using System.Diagnostics.CodeAnalysis;

public ref struct ParsingContext
{
	private ReadOnlySpan<char> _originalInput;

	public ReadOnlySpan<char> Input { get; private set; }

	public ParsingContext(ReadOnlySpan<char> input)
	{
		Input = input;
		_originalInput = input;
	}

	public class InputPosition
	{
		public int Index { get; set; }
		public int Row { get; set; } = 1;
		public int Column { get; set; } = 1;
	}

	public InputPosition Position { get; } = new();

	public List<string> Errors { get; } = [];

	public readonly void AddError(string message) => Errors.Add($"({Position.Row}:{Position.Column}) {message}");

	public void MoveAheadInput(int length)
	{
		for (int i = 0; i < length; i++)
		{
			if (Input[i] == '\n')
			{
				Position.Row++;
				Position.Column = 1;
			}
			else
			{
				Position.Column++;
			}
		}
		Input = Input[length..];
		Position.Index += length;
	}

	public readonly bool IsInputFinished() => Input.Length == 0;

	public override readonly string ToString() => $"Pos: {Position.Index} ({Position.Row}:{Position.Column}), Next: '{(IsInputFinished() ? "<EOF>" : Input[..Math.Min(20, Input.Length)])}'";

	public class ParsedTokenList()
	{
		private List<ParsedToken> _parsedTokens { get; } = [];
		private int _currentTokenIndex = 0;

		public ParsedToken GetNext()
		{
			if (_currentTokenIndex >= _parsedTokens.Count)
			{
				throw new InvalidOperationException("No more tokens available.");
			}

			return _parsedTokens[_currentTokenIndex++];
		}

		public bool PeekNext([NotNullWhen(true)] out ParsedToken? token, int tokenIndex = 0)
		{
			if (_currentTokenIndex + tokenIndex < _parsedTokens.Count)
			{
				token = _parsedTokens[_currentTokenIndex + tokenIndex];
				return true;
			}

			token = null;
			return false;
		}

		public void Move(int matchLength)
		{
			if (_currentTokenIndex + matchLength > _parsedTokens.Count)
			{
				throw new InvalidOperationException("Cannot move ahead tokens beyond the available tokens.");
			}

			_currentTokenIndex += matchLength;
		}

		public bool Finished() => _currentTokenIndex < _parsedTokens.Count;

		public void Add(ParsedToken parsedToken) => _parsedTokens.Add(parsedToken);

		public int GetId() => _parsedTokens.Count + 1;
	}

	public readonly ParsedTokenList Tokens { get; } = new();

	public readonly ReadOnlySpan<char> GetTokenValue(ParsedToken parsedToken) => _originalInput[parsedToken.StartAt..parsedToken.EndAt];

	public ReadOnlySpan<char> GetValueBetween(int startAt, int endAt) => _originalInput[startAt..endAt];
}

