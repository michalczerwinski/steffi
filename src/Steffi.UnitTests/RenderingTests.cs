namespace Steffi.UnitTests;

public class RenderingTests : SteffiParserTestsBase
{
	[Test, DisplayName("Renders basic canvas with rectangle and text")]
	public async Task RendersBasicCanvasWithShapes() => await RendersSvgCorrectly("SvgGeneration01");

	[Test, DisplayName("Renders empty canvas")]
	public async Task RendersEmptyCanvas() => await RendersSvgCorrectly("SvgGeneration02");

	[Test, DisplayName("Renders rectangle with all properties")]
	public async Task RendersRectangleWithAllProperties() => await RendersSvgCorrectly("SvgGeneration03");

	[Test, DisplayName("Renders text with all properties")]
	public async Task RendersTextWithAllProperties() => await RendersSvgCorrectly("SvgGeneration04");

	[Test, DisplayName("Renders text with multiline spans")]
	public async Task RendersTextWithMultiline() => await RendersSvgCorrectly("SvgGeneration05");

	[Test, DisplayName("Renders horizontal stack with multiple rectangles")]
	public async Task RendersHorizontalStackWithRectangles() => await RendersSvgCorrectly("SvgGeneration06");

	[Test, DisplayName("Renders vertical stack with multiple rectangles")]
	public async Task RendersVerticalStackWithRectangles() => await RendersSvgCorrectly("SvgGeneration07");

	[Test, DisplayName("Renders horizontal stack with mixed elements")]
	public async Task RendersHorizontalStackWithMixedElements() => await RendersSvgCorrectly("SvgGeneration08");

	[Test, DisplayName("Renders vertical stack with mixed elements")]
	public async Task RendersVerticalStackWithMixedElements() => await RendersSvgCorrectly("SvgGeneration09");

	[Test, DisplayName("Renders empty horizontal stack")]
	public async Task RendersEmptyHorizontalStack() => await RendersSvgCorrectly("SvgGeneration10");

	[Test, DisplayName("Renders empty vertical stack")]
	public async Task RendersEmptyVerticalStack() => await RendersSvgCorrectly("SvgGeneration11");

	[Test, DisplayName("Renders nested canvas in vertical stack")]
	public async Task RendersNestedCanvas() => await RendersSvgCorrectly("SvgGeneration12");

	[Test, DisplayName("Renders horizontal stack inside canvas")]
	public async Task RendersHorizontalStackInCanvas() => await RendersSvgCorrectly("SvgGeneration13");

	[Test, DisplayName("Renders vertical stack inside canvas")]
	public async Task RendersVerticalStackInCanvas() => await RendersSvgCorrectly("SvgGeneration14");

	[Test, DisplayName("Renders horizontal stack inside vertical stack")]
	public async Task RendersNestedStacks() => await RendersSvgCorrectly("SvgGeneration15");

	[Test, DisplayName("Renders deep nesting of containers")]
	public async Task RendersDeepNesting() => await RendersSvgCorrectly("SvgGeneration16");

	[Test, DisplayName("Renders multiple containers side by side")]
	public async Task RendersMultipleContainersSideBySide() => await RendersSvgCorrectly("SvgGeneration17");

	[Test, DisplayName("Renders grid-like layout")]
	public async Task RendersGridLayout() => await RendersSvgCorrectly("SvgGeneration18");

	[Test, DisplayName("Renders card layout")]
	public async Task RendersCardLayout() => await RendersSvgCorrectly("SvgGeneration19");

	[Test, DisplayName("Renders dashboard layout with multiple widgets")]
	public async Task RendersDashboardLayout() => await RendersSvgCorrectly("SvgGeneration20");

	[Test, DisplayName("Renders rectangle with rounded corners")]
	public async Task RendersRectangleWithRoundedCorners() => await RendersSvgCorrectly("SvgGeneration21");

	[Test, DisplayName("Renders rectangle with custom colors")]
	public async Task RendersRectangleWithCustomColors() => await RendersSvgCorrectly("SvgGeneration22");

	[Test, DisplayName("Renders text with custom font")]
	public async Task RendersTextWithCustomFont() => await RendersSvgCorrectly("SvgGeneration23");

	[Test, DisplayName("Renders text with color")]
	public async Task RendersTextWithColor() => await RendersSvgCorrectly("SvgGeneration24");

	[Test, DisplayName("Renders single rectangle without canvas")]
	public async Task RendersSingleRectangle() => await RendersSvgCorrectly("SvgGeneration25");

	[Test, DisplayName("Renders single text without canvas")]
	public async Task RendersSingleText() => await RendersSvgCorrectly("SvgGeneration26");

	[Test, DisplayName("Renders large dimensions")]
	public async Task RendersLargeDimensions() => await RendersSvgCorrectly("SvgGeneration27");

	[Test, DisplayName("Renders small dimensions")]
	public async Task RendersSmallDimensions() => await RendersSvgCorrectly("SvgGeneration28");

	[Test, DisplayName("Renders zero dimensions")]
	public async Task RendersZeroDimensions() => await RendersSvgCorrectly("SvgGeneration29");

	[Test, DisplayName("Renders simple form layout")]
	public async Task RendersSimpleForm() => await RendersSvgCorrectly("SvgGeneration30");

	[Test, DisplayName("Renders button group")]
	public async Task RendersButtonGroup() => await RendersSvgCorrectly("SvgGeneration31");

	[Test, DisplayName("Renders header body footer layout")]
	public async Task RendersHeaderBodyFooter() => await RendersSvgCorrectly("SvgGeneration32");

	[Test, DisplayName("Renders sidebar layout")]
	public async Task RendersSidebarLayout() => await RendersSvgCorrectly("SvgGeneration33");

	[Test, DisplayName("Renders icon grid")]
	public async Task RendersIconGrid() => await RendersSvgCorrectly("SvgGeneration34");

	[Test, DisplayName("Renders rectangle with stroke width")]
	public async Task RendersRectangleWithStrokeWidth() => await RendersSvgCorrectly("SvgGeneration35");

	[Test, DisplayName("Renders canvas with border false and custom padding")]
	public async Task RendersCanvasWithoutBorderAndPadding() => await RendersSvgCorrectly("SvgGeneration36");

	[Test, DisplayName("Renders horizontal stack without border")]
	public async Task RendersHorizontalStackWithoutBorder() => await RendersSvgCorrectly("SvgGeneration37");

	[Test, DisplayName("Renders vertical stack with custom padding")]
	public async Task RendersVerticalStackWithCustomPadding() => await RendersSvgCorrectly("SvgGeneration38");

	[Test, DisplayName("Renders canvas with custom fill and stroke")]
	public async Task RendersCanvasWithFillAndStroke() => await RendersSvgCorrectly("SvgGeneration39");

	[Test, DisplayName("Renders horizontal stack with custom fill and stroke")]
	public async Task RendersHorizontalStackWithFillAndStroke() => await RendersSvgCorrectly("SvgGeneration40");

	[Test, DisplayName("Renders vertical stack with custom fill and stroke")]
	public async Task RendersVerticalStackWithFillAndStroke() => await RendersSvgCorrectly("SvgGeneration41");

	[Test, DisplayName("Renders canvas with custom stroke width")]
	public async Task RendersCanvasWithStrokeWidth() => await RendersSvgCorrectly("SvgGeneration42");

	[Test, DisplayName("Renders horizontal stack with custom stroke width")]
	public async Task RendersHorizontalStackWithStrokeWidth() => await RendersSvgCorrectly("SvgGeneration43");

	[Test, DisplayName("Renders vertical stack with custom stroke width")]
	public async Task RendersVerticalStackWithStrokeWidth() => await RendersSvgCorrectly("SvgGeneration44");
}
