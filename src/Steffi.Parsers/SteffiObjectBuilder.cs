using Steffi.Parsers.Model;

namespace Steffi.Parsers;

public static class SteffiObjectBuilder
{
	public static SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name) => tokenType switch
	{
		"Node" => new Node { Name = name.ToString() },
		"Graph" => new Graph { Name = name.ToString() },
		_ => null,
	};

	public static void SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is Node node)
		{
			if (propertyName.SequenceEqual("label"))
			{
				node.Label = value.ToString();
			}

		}
		else if (steffiObject is Graph graph)
		{
			if (propertyName.SequenceEqual("TODO"))
			{
			}
		}
	}
}