using Spectre.Console;
using Steffi.Cli.Helpers;
using Steffi.Parsers;
using System.CommandLine;

namespace Steffi.Cli.Commands;

public class StructureCommand : Command
{
	public StructureCommand() : base("structure", "Parse a Steffi document and render its structure.")
	{
		var inputFileOption = new Option<FileInfo>(
			name: "--input-file",
			description: "Path to the .stf document to parse and visualize.")
		{
			IsRequired = true
		};

		AddOption(inputFileOption);

		this.SetHandler(async (FileInfo inputFile) =>
		{
			if (!inputFile.Exists)
			{
				AnsiConsole.MarkupLine($"[red]File not found:[/] {Markup.Escape(inputFile.FullName)}");
				Environment.ExitCode = 1;
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

				Environment.ExitCode = 1;
				return;
			}

			AnsiConsole.MarkupLine("[green]Parsing succeeded[/]");
			var tree = SteffiTreeRenderer.CreateTree(document!);
			AnsiConsole.Write(tree);
			Environment.ExitCode = 0;
		}, inputFileOption);
	}
}
