using Steffi.Models;
using Steffi.Models.Containers;

namespace Steffi.UnitTests;

public class SteffiParserTests : SteffiParserTestsBase
{

	[Test, DisplayName("Compiles comments whitespace and single object without name"),]
	public async Task CompilesCase01() => await CompilesWithoutError(
		"""
		//this is a test

		Canvas
		{
			//to do
		}
		""");

	[Test, DisplayName("Compiles comments whitespace and single object with name")]
	public async Task CompilesCase02() => await CompilesWithoutError(
		"""
		//this is a test

		Canvas cloud
		{
			//to do
		}
		""");


	[Test, DisplayName("Compiles single line")]
	public async Task CompilesCase03() => await CompilesWithoutError("HorizontalStack stack {Rectangle{}HorizontalStack{HorizontalStack{}HorizontalStack{}}}");

	[Test, DisplayName("Compiles nested objects")]
	public async Task CompilesCase04() => await CompilesWithoutError(
		"""
		Canvas cloud
		{
			Rectangle nested{ }
		}
		""");

	[Test, DisplayName("Fails when nested closing missing")]
	public async Task FailsCase01() => await FailsWithError(
		"""
		//this is a test

		Canvas cloud
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
			Canvas name
			{
		""",
		"(2,3): Unexpected end of file, object not closed");

	[Test, DisplayName("Fails when object closed too many times")]
	public async Task FailsCase04() => await FailsWithError(
		"""
		Canvas name
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
		
		Canvas
		{
			/* this is a block comment */

		}
		""");

	[Test, DisplayName("Compiles with simple property assignment")]
	public async Task CompilesCase06()
	{
		var result = await CompilesWithoutError(
			"""
			Rectangle n1
			{
				width: 12;
			}
			""");

		await Assert.That(result.Document).IsNotNull();
		await Assert.That(result.Document!.Children.Count).IsEqualTo(1);
		await Assert.That(result.Document!.Children[0]).IsTypeOf<Rectangle>();
		await Assert.That(((Rectangle)result.Document!.Children[0]).Width).IsEqualTo(12);
	}

	[Test, DisplayName("Compiles mixed property and nesting"),]
	public async Task CompilesCase07()
	{
		var result = await CompilesWithoutError(
		"""
		Canvas canvas
		{
			Rectangle r1 {x: 0; y: 0;}
			width: 13;
			Rectangle r1 {x: 0; y: 0;}
		}
		""");

		await Assert.That(result.Document).IsNotNull();
		await Assert.That(result.Document!.Children.Count).IsEqualTo(1);
		await Assert.That(((Canvas)result.Document!.Children[0]).Children.Count).IsEqualTo(2);
	}

	[Test, DisplayName("Fails when object cannot be nested")]
	public async Task FailsCase08() => await FailsWithError(
		"""
		Rectangle name
		{
			HorizontalStack child
			{
			}
		}		
		""",
		"(3,2): Cannot nest children in Rectangle");

	[Test, DisplayName("Compiles basic line with x2 and y2")]
	public async Task CompilesBasicLine()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x2: 100;
				y2: 50;
			}
			""");

		await Assert.That(result.Document).IsNotNull();
		await Assert.That(result.Document!.Children.Count).IsEqualTo(1);
		await Assert.That(result.Document!.Children[0]).IsTypeOf<Line>();

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.X2).IsEqualTo(100);
		await Assert.That(line.Y2).IsEqualTo(50);
	}

	[Test, DisplayName("Line has null x1 and y1 by default")]
	public async Task LineHasNullX1Y1ByDefault()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x2: 200;
				y2: 100;
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.X1).IsNull();
		await Assert.That(line.Y1).IsNull();
	}

	[Test, DisplayName("Compiles line with all coordinate properties")]
	public async Task CompilesLineWithAllCoordinates()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x1: 10;
				y1: 20;
				x2: 150;
				y2: 75;
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.X1).IsEqualTo(10);
		await Assert.That(line.Y1).IsEqualTo(20);
		await Assert.That(line.X2).IsEqualTo(150);
		await Assert.That(line.Y2).IsEqualTo(75);
	}

	[Test, DisplayName("Compiles line with custom stroke and fill")]
	public async Task CompilesLineWithStrokeAndFill()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x2: 100;
				y2: 50;
				stroke: "red";
				strokeWidth: "3";
				fill: "none";
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.Stroke).IsEqualTo("red");
		await Assert.That(line.StrokeWidth).IsEqualTo("3");
		await Assert.That(line.Fill).IsEqualTo("none");
	}

	[Test, DisplayName("Line inherits default fill and stroke from Shape")]
	public async Task LineHasDefaultFillAndStroke()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x2: 50;
				y2: 50;
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.Fill).IsEqualTo("white");
		await Assert.That(line.Stroke).IsEqualTo("black");
	}

	[Test, DisplayName("Compiles named line")]
	public async Task CompilesNamedLine()
	{
		var result = await CompilesWithoutError(
			"""
			Line myLine {
				x2: 80;
				y2: 40;
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.Name).IsEqualTo("myLine");
	}

	[Test, DisplayName("Compiles line with stroke opacity and line cap")]
	public async Task CompilesLineWithStrokeOpacityAndLineCap()
	{
		var result = await CompilesWithoutError(
			"""
			Line {
				x2: 100;
				y2: 50;
				strokeOpacity: "0.5";
				strokeLineCap: "round";
			}
			""");

		var line = (Line)result.Document!.Children[0];
		await Assert.That(line.StrokeOpacity).IsEqualTo("0.5");
		await Assert.That(line.StrokeLineCap).IsEqualTo("round");
	}
}