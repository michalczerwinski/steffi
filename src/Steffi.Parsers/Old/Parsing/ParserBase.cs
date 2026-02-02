using Steffi.Parsers.Model;
using Steffi.Parsers.Old.Parsers;

namespace Steffi.Parsers.Old.Parsing;

public abstract class ParserBase<TModel> where TModel : SteffiDocument
{
	public async Task<(SteffiDocument?, List<string> Errors)> ParseFromFileAsync(string fileName)
	{
		string content = await File.ReadAllTextAsync(fileName);

		return Parse(content);
	}

	public (SteffiDocument?, List<string> Errors) Parse(string content)
	{
		return new SteffiParser().Parse(content);

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

