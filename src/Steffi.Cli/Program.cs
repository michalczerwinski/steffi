using Steffi.Cli.Commands;
using System.CommandLine;

var rootCommand = new RootCommand("Steffi CLI – work with Steffi graph documents");

rootCommand.AddCommand(new StructureCommand());
rootCommand.AddCommand(new ValidateCommand());

return await rootCommand.InvokeAsync(args);
