using Steffi.Parsers.Parsers;

namespace Steffi.Parsers.Parsing;

public abstract class ParserBase<TModel>
{
	public async Task<(TModel?, List<string> Errors)> ParseFromFileAsync(string fileName)
	{
		string content = await File.ReadAllTextAsync(fileName);

		return Parse(content);
	}

	public (TModel?, List<string> Errors) Parse(string content)
	{
		var parsingContext = new ParsingContext(content);
		var lexer = new SteffiLexer();
		lexer.GenerateTokens(ref parsingContext);

		if (parsingContext.Errors.Any())
		{
			return (default, parsingContext.Errors);
		}

		var result = GenerateSyntaxTree(parsingContext);
		result.Errors.InsertRange(0, parsingContext.Errors);

		return result;
	}

	protected abstract (TModel?, List<string> Errors) GenerateSyntaxTree(ParsingContext parsingContext);
}

