using Spectre.Console;
using Spectre.Console.Rendering;
using Steffi.Models;
using Steffi.Models.Interfaces;

namespace Steffi.Cli.Helpers;

public static class SteffiConsole
{
	private static Panel CreatePanel(SteffiObject steffiObject)
	{
		IRenderable body = steffiObject is IParentObject parent && parent.Children.Count > 0
			? BuildChildren(parent.Children)
			: new Markup("[grey italic]No children[/]");

		var (title, color) = GetTitleColor(steffiObject);
		var headerMarkup = $"[{color}]{Markup.Escape(title)}[/]";

		return new Panel(body)
			.Header(new PanelHeader(headerMarkup, Justify.Left))
			.Border(BoxBorder.Square)
			.BorderStyle(Style.Parse(color))
			.Expand();
	}

	private static IRenderable BuildChildren(List<SteffiObject> children)
	{
		if (children.Count == 1)
		{
			return CreatePanel(children[0]);
		}

		var panels = new List<IRenderable>(children.Count);
		foreach (var child in children)
		{
			panels.Add(CreatePanel(child));
		}

		return new Columns(panels.ToArray());
	}

	private static (string Title, string Color) GetTitleColor(SteffiObject steffiObject)
	{
		var color = steffiObject switch
		{
			SteffiDocument => "dodgerblue1",
			_ => "white"
		};

		return (GetTitle(steffiObject), color);
	}

	private static string GetTitle(SteffiObject steffiObject)
	{
		return steffiObject switch
		{
			SteffiDocument => "Steffi Document",
			_ => $"{steffiObject.GetType().Name}: {FormatName(steffiObject.Name)}"
		};
	}

	private static string FormatName(string? name) => string.IsNullOrWhiteSpace(name) ? "<unnamed>" : name.Trim();
}
