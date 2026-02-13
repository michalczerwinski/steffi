using Spectre.Console;
using Steffi.Models;
using Steffi.Models.Interfaces;

namespace Steffi.Cli.Helpers;

public static class SteffiTreeRenderer
{
	public static Tree CreateTree(SteffiObject root)
	{
		var tree = new Tree(GetColoredTitle(root));
		Populate(tree, root);
		return tree;
	}

	private static void Populate(Tree tree, SteffiObject steffiObject)
	{
		if (steffiObject is not IParentObject parent)
		{
			return;
		}

		foreach (var child in parent.Children)
		{
			var childNode = tree.AddNode(GetColoredTitle(child));
			Populate(childNode, child);
		}
	}

	private static void Populate(TreeNode node, SteffiObject steffiObject)
	{
		if (steffiObject is not IParentObject parent)
		{
			return;
		}

		foreach (var child in parent.Children)
		{
			var childNode = node.AddNode(GetColoredTitle(child));
			Populate(childNode, child);
		}
	}

	private static string GetColoredTitle(SteffiObject steffiObject)
	{
		var (title, color) = GetTitleAndColor(steffiObject);
		return $"[{color}]{Markup.Escape(title)}[/]";
	}

	private static (string Title, string Color) GetTitleAndColor(SteffiObject steffiObject)
	{
		return steffiObject switch
		{
			SteffiDocument => ("Document", "dodgerblue1"),
			INamedObject named => ($"{steffiObject.GetType().Name}: {FormatName(named.Name)}", "white"),
			_ => (steffiObject.GetType().Name, "white")
		};
	}

	private static string FormatName(string? name)
	{
		return string.IsNullOrWhiteSpace(name) ? "<unnamed>" : name.Trim();
	}
}
