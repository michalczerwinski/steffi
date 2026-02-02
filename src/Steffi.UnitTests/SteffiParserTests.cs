using Steffi.Parsers.Model;

namespace Steffi.UnitTests;

public class SteffiParserTests : SteffiParserTestsBase
{

	[Test, DisplayName("Compiles comments whitespace and single object without name"),]
	public async Task CompilesCase01() => await CompilesWithoutError(
		"""
		//this is a test

		Graph
		{
			//to do
		}
		""");

	[Test, DisplayName("Compiles comments whitespace and single object with name")]
	public async Task CompilesCase02() => await CompilesWithoutError(
		"""
		//this is a test

		Graph cloud
		{
			//to do
		}
		""");


	[Test, DisplayName("Compiles single line")]
	public async Task CompilesCase03() => await CompilesWithoutError("Graph graph {Node{}Graph{Graph{}Graph{}}}");

	[Test, DisplayName("Compiles nested objects")]
	public async Task CompilesCase04() => await CompilesWithoutError(
		"""
		Graph cloud
		{
			Node nested{ }
		}
		""");

	[Test, DisplayName("Fails when nested closing missing")]
	public async Task FailsCase01() => await FailsWithError(
		"""
		//this is a test

		Graph cloud
		{$ERROR
			//to do
		}
		""",
		"(4,2): Unexpected expression");

	[Test, DisplayName("Fails when unknown type used")]
	public async Task FailsCase02() => await FailsWithError(
		"""
		UnknownType name
		{
		}
		""",
		"(1,1): Unknown type 'UnknownType'");

	[Test, DisplayName("Fails when object not finished")]
	public async Task FailsCase03() => await FailsWithError(
		"""
			Graph name
			{
		""",
		"(2,3): Unexpected end of file, object not closed");

	[Test, DisplayName("Fails when object closed too many times")]
	public async Task FailsCase04() => await FailsWithError(
		"""
		Graph name
		{
		}
		}
		""",
		"(4,2): Unexpected end of file, object not closed");

	[Test, DisplayName("Compiles block comments")]
	public async Task CompilesCase05() => await CompilesWithoutError(
		"""
		/* this is a block comment */
		/* another one */
		
		Graph
		{
			/* this is a block comment */

		}
		""");

	[Test, DisplayName("Compiles with simple property assignment")]
	public async Task CompilesCase06()
	{
		var result = await CompilesWithoutError(
			"""
			Node n1
			{
				label: 12;
			}
			""");

		await Assert.That(result.Document).IsNotNull();
		await Assert.That(result.Document!.Children.Count).IsEqualTo(1);
		await Assert.That(result.Document!.Children[0]).IsTypeOf<Node>();
		await Assert.That(((Node)result.Document!.Children[0]).Label).IsEqualTo("12");
	}

	[Test, DisplayName("Compiles mixed property and nesting"),]
	public async Task CompilesCase07()
	{
		var result = await CompilesWithoutError(
		"""
		Graph
		{
			Node n1 {}
			layout: standard;
			Node n2 {}
		}
		""");

		await Assert.That(result.Document).IsNotNull();
		await Assert.That(result.Document!.Children.Count).IsEqualTo(1);
		await Assert.That(((Graph)result.Document!.Children[0]).Children.Count).IsEqualTo(2);
	}
}