using Spectre.Console;
using Steffi.Parsers;
using Steffi.Renderers.Svg;
using System.CommandLine;

namespace Steffi.Cli.Commands;

public class GenerateCommand : Command
{
	public GenerateCommand() : base("generate", "Generate output from a Steffi document in the specified format.")
	{
		var inputFileOption = new Option<FileInfo>(
			name: "--input-file",
			description: "Path to the .stf document to generate from.")
		{
			IsRequired = true
		};

		var formatOption = new Option<string>(
			name: "--format",
			description: "Output format (e.g., svg).")
		{
			IsRequired = true
		};

		AddOption(inputFileOption);
		AddOption(formatOption);

		this.SetHandler(async (FileInfo inputFile, string format) =>
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

			if (format.Equals("svg", StringComparison.OrdinalIgnoreCase))
			{
				var renderer = new SvgRenderer();
				var output = renderer.RenderDocument(document!);

				AnsiConsole.WriteLine(output);
				Environment.ExitCode = 0;
			}
			else
			{
				AnsiConsole.MarkupLine($"[red]Error: Unsupported format '{Markup.Escape(format)}'. Only 'svg' is currently supported.[/]");
				Environment.ExitCode = 1;
			}
		}, inputFileOption, formatOption);
	}
}
