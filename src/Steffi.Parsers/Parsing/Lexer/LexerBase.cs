namespace Steffi.Parsers.Parsing;

public abstract class LexerBase
{
	public abstract Token[] KnownTokens { get; }

	public virtual bool ShouldBeIgnored(Token token) => false;

	public void GenerateTokens(ref ParsingContext parsingContext)
	{
		while (!parsingContext.IsInputFinished())
		{
			bool matched = false;

			foreach (var token in KnownTokens)
			{
				var parsedToken = TryToken(token, ref parsingContext);

				if (parsedToken is not null)
				{
					matched = true;
					if (!ShouldBeIgnored(token))
					{
					    parsingContext.Tokens.Add(parsedToken);
					}
					break;
				}
			}

			if (!matched)
			{
				parsingContext.AddError($"Unrecognized token");
				break;
			}
		}
	}

	protected ParsedToken? TryToken(Token tokenParser, ref ParsingContext parsingContext)
	{
		var attempt = tokenParser.TryParse(parsingContext.Input);

		if (attempt.Success)
		{
			var positionBefore = parsingContext.Position;
			var rowBefore = parsingContext.PositionRow;
			var columnBefore = parsingContext.PositionColumn;

			parsingContext.MoveAheadInput(attempt.Length);

			var tokenId = parsingContext.Tokens.Count + 1;
			return new ParsedToken(tokenId, tokenParser, positionBefore, parsingContext.Position, rowBefore, columnBefore);
		}

		return null;
	}
}
