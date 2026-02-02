using Steffi.Parsers;
using Steffi.Models;

namespace Steffi.UnitTests;

public class SteffiParserTestsBase
{
	protected async Task<(SteffiDocument? Document, List<string> Errors)> FailsWithError(string input, string expectedError)
	{
		var parser = new SteffiParser();
		var result = parser.Parse(input);

		await Assert.That(result.Errors.Count).IsGreaterThan(0);
		await Assert.That(result.Errors.First()).IsEqualTo(expectedError);

		return result;
	}

	protected async Task<(SteffiDocument? Document, List<string> Errors)> CompilesWithoutError(string input)
	{
		var parser = new SteffiParser();
		var result = parser.Parse(input);

		await Assert.That(result.Errors.FirstOrDefault()).IsEqualTo(null);

		return result;
	}
}
