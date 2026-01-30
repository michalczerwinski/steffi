using Spectre.Console;
using Steffi.Cli.Helpers;
using Steffi.Parsers.Parsers;

Console.WriteLine("Steffi, version 0.1");

var input =
"""
//this is a test
//another comment
Graph {
	Graph anotherNested {
		Node {
		}
	}
}

Graph named
{
	Node nested {
	}
}
Node
{
}

Node namedNode
{
}

""";

var (document, errors) = new SteffiParser().Parse(input);

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
