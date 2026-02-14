using Steffi.Parsers;
using Steffi.Models;
using Steffi.Renderers.Svg;

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

	protected async Task RendersSvgCorrectly(string testName)
	{
		var inputFile = Path.Combine("RenderingTests", $"{testName}.input.stf");
		var expectedFile = Path.Combine("RenderingTests", $"{testName}.expected.svg");

		var stfContent = await File.ReadAllTextAsync(inputFile);
		var expectedSvg = await File.ReadAllTextAsync(expectedFile);

		var (document, errors) = await CompilesWithoutError(stfContent);
		await Assert.That(document).IsNotNull();
		await Assert.That(errors).IsEmpty();

		var renderer = new SvgRenderer();
		var actualSvg = renderer.RenderDocument(document!);
		await Assert.That(actualSvg.Trim()).IsEqualTo(expectedSvg.Trim());
	}
}
