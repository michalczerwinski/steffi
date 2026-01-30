using Spectre.Console;
using Steffi.Cli.Helpers;
using Steffi.Parsers.Parsers;

Console.WriteLine("Steffi, version 0.1");

var (document, errors) = await new SteffiParser().ParseFromFileAsync(args[0]);

if (errors.Count != 0)
{
	foreach (var error in errors)
	{
		Console.WriteLine(error);
	}
}
else
{
	AnsiConsole.Markup("Parsing: [green][[OK]][/]\n");
	SteffiConsole.Print(document!);
}
