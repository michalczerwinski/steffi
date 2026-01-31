using Spectre.Console;
using Steffi.Cli.Helpers;
using Steffi.Parsers.Parsers;
using System.CommandLine;

namespace Steffi.Cli.Commands;

public class StructureCommand : Command
{
	public StructureCommand() : base("structure", "Parse a Steffi document and render its structure.")
	{
		var inputArgument = new Argument<FileInfo>(
			name: "input",
			description: "Path to the .stf document to parse and visualize.");

		AddArgument(inputArgument);

		this.SetHandler(async (FileInfo inputFile) =>
		{
			if (!inputFile.Exists)
			{
				AnsiConsole.MarkupLine($"[red]File not found:[/] {Markup.Escape(inputFile.FullName)}");
				return;
			}

			var parser = new SteffiParser();
			var (document, errors) = await parser.ParseFromFileAsync(inputFile.FullName);

			if (errors.Count > 0)
			{
				AnsiConsole.MarkupLine("[red]Parsing failed[/]");
				foreach (var error in errors)
				{
					AnsiConsole.MarkupLine($"[red]- {Markup.Escape(error)}[/]");
				}

				return;
			}

			AnsiConsole.MarkupLine("[green]Parsing succeeded[/]");
			var tree = SteffiTreeRenderer.CreateTree(document!);
			AnsiConsole.Write(tree);
		}, inputArgument);
	}
}
