using Spectre.Console;
using Steffi.Parsers.Parsers;
using System.CommandLine;

namespace Steffi.Cli.Commands;

public class ValidateCommand : Command
{
	public ValidateCommand() : base("validate", "Parse and validate a Steffi document.")
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
				Environment.ExitCode = 1;
				return;
			}

			var parser = new SteffiParser();
			var (document, errors) = await parser.ParseFromFileAsync(inputFile.FullName);

			if (errors.Count > 0)
			{
				AnsiConsole.MarkupLine("[red]Validation failed[/]");
				foreach (var error in errors)
				{
					AnsiConsole.MarkupLine($"[red]- {Markup.Escape(error)}[/]");
				}

				Environment.ExitCode = 1;
				return;
			}

			AnsiConsole.MarkupLine("[green]Validation succeeded[/]");
			Environment.ExitCode = 0;
		}, inputArgument);
	}
}
