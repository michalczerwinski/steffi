using Steffi.Parsers.Model;

namespace Steffi.Cli.Helpers;

public static class SteffiConsole
{
    public static void Print(SteffiObject steffiObject, int indent = 0)
    {
        var namedObject = steffiObject as INamedObject;
        Console.WriteLine(new string(' ', indent) + steffiObject.GetType().Name + $" {namedObject?.Name}");
        if (steffiObject is IParentObject parentObject)
        {
            foreach (var o in parentObject.Children)
            {
                Print(o, indent + 2);
            }
        }
    }
}
