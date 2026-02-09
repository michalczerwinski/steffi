using Steffi.Cli.Helpers;
using System.CommandLine;

namespace Steffi.Cli.Commands;

public class InteractiveCommand : Command
{
	public InteractiveCommand() : base("interactive", "Start an interactive preview server with live reload.")
	{
		var inputFileOption = new Option<FileInfo>(
			name: "--input-file",
			description: "Path to the .stf document to preview.")
		{
			IsRequired = true
		};

		var portOption = new Option<int>(
			name: "--port",
			description: "Port number for the preview server.",
			getDefaultValue: () => 5100);

		AddOption(inputFileOption);
		AddOption(portOption);

		this.SetHandler(async (FileInfo inputFile, int port) =>
		{
			if (!inputFile.Exists)
			{
				Console.WriteLine($"File not found: {inputFile.FullName}");
				Environment.ExitCode = 1;
				return;
			}

			using var server = new PreviewServer(inputFile.FullName, port);
			Environment.ExitCode = await server.StartAsync();

		}, inputFileOption, portOption);
	}
}
