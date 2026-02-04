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

		var outputOption = new Option<string?>(
			name: "--output",
			description: "Output path. Can be a directory or full file path. If not specified, outputs to the same location as input with changed extension.");

		AddOption(inputFileOption);
		AddOption(formatOption);
		AddOption(outputOption);

		this.SetHandler(async (FileInfo inputFile, string format, string? output) =>
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
				var generatedContent = renderer.RenderDocument(document!);

				var outputPath = DetermineOutputPath(inputFile, format, output);

				try
				{
					await File.WriteAllTextAsync(outputPath, generatedContent);
					AnsiConsole.MarkupLine($"[green]Generated successfully:[/] {Markup.Escape(outputPath)}");
					Environment.ExitCode = 0;
				}
				catch (Exception ex)
				{
					AnsiConsole.MarkupLine($"[red]Error writing output file:[/] {Markup.Escape(ex.Message)}");
					Environment.ExitCode = 1;
				}
			}
			else
			{
				AnsiConsole.MarkupLine($"[red]Error: Unsupported format '{Markup.Escape(format)}'. Only 'svg' is currently supported.[/]");
				Environment.ExitCode = 1;
			}
		}, inputFileOption, formatOption, outputOption);
	}

	private static string DetermineOutputPath(FileInfo inputFile, string format, string? output)
	{
		var extension = $".{format.ToLowerInvariant()}";

		if (string.IsNullOrWhiteSpace(output))
		{
			// No output specified: same location, changed extension
			return Path.ChangeExtension(inputFile.FullName, extension);
		}

		// Check if output is a directory
		if (Directory.Exists(output) || output.EndsWith(Path.DirectorySeparatorChar) || output.EndsWith(Path.AltDirectorySeparatorChar))
		{
			// Output is a directory: use input filename with changed extension
			var fileName = Path.GetFileNameWithoutExtension(inputFile.Name) + extension;
			return Path.Combine(output, fileName);
		}

		// Output is a full file path: use as-is
		return output;
	}
}
