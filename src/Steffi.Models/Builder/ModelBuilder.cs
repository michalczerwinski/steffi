using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static class ModelBuilder
{
	public static SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name) => tokenType switch
	{
		"Node" => new Node { Name = name.ToString() },
		"Graph" => new Graph { Name = name.ToString() },
		"Edge" => new Edge { Name = name.ToString() },
		_ => null,
	};

	public static void SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is IParentObject parentObject)
		{
			if (propertyName.SequenceEqual("layout"))
			{
				parentObject.Layout = Enum.Parse<LayoutType>(value);
			}
		}

		if (steffiObject is ILabeledObject labeledObject)
		{
			if (propertyName.SequenceEqual("label"))
			{
				labeledObject.Label = value.ToString();
			}
			else if (propertyName.SequenceEqual("fontColor"))
			{
				labeledObject.FontColor = value.ToString();
			}
		}

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
