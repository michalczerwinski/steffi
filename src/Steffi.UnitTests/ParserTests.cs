using Steffi.Parsers.Model;
using Steffi.Parsers.Parsers;

namespace Steffi.UnitTests;

public class ParserTests
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

	[Test]
	public async Task Compile_comments_whitespace_and_single_object_without_name()
		=> await CompilesWithoutError("""

			//this is a test

			Graph
			{
				//to do
			}
		""");

	[Test]
	public async Task Compile_comments_whitespace_and_single_object_with_name()
		=> await CompilesWithoutError("""

			//this is a test

			Graph cloud
			{
				//to do
			}
		""");


	[Test]
	public async Task Compile_single_line()
		=> await CompilesWithoutError("Graph graph {Node{}Graph{Graph{}Graph{}}}");

	[Test]
	public async Task Compile_nested_objects()
		=> await CompilesWithoutError("""
			Graph cloud
			{
				Node nested{ }
			}
		""");

	[Test]
	public async Task Fail_when_nested_closing_missing()
		=> await FailsWithError("""

			//this is a test

			Graph cloud
			{$ERROR
				//to do
			}
		""",
		"(5:3) Unrecognized token");

	[Test]
	public async Task Fail_when_unknownTypeUsed()
		=> await FailsWithError("""
			UnknownType name
			{
			}
		""",
		"(1:2) Unknown type 'UnknownType'");

	[Test]
	public async Task Fail_when_object_not_finished()
		=> await FailsWithError("""
			Graph name
			{
		""",
		"Unexpected end of file, object not closed");

	[Test]
	public async Task Fail_when_object_closed_too_many_times()
		=> await FailsWithError("""
			Graph name
			{
			}
			}
		""",
		"Unexpected end of file, object not closed");

	[Test]
	public async Task Compile_block_comments()
		=> await CompilesWithoutError("""

				/* this is a block comment */
				/* another one */
		
				Graph
				{
					/* this is a block comment */

				}
			""");

}
