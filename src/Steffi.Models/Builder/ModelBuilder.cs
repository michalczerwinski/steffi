using Steffi.Models.Interfaces;

namespace Steffi.Models.Builder;

public static class ModelBuilder
{
	public static SteffiObject? CreateObjectFactory(ReadOnlySpan<char> tokenType, ReadOnlySpan<char> name, IParentObject parentObject) => tokenType switch
	{
		nameof(Canvas) => new Canvas { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(HorizontalStack) => new HorizontalStack { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(VerticalStack) => new VerticalStack { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(Rectangle) => new Rectangle { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
		nameof(Text) => new Text { Name = name.ToString(), Parent = parentObject, ParentProperties = parentObject.CreateContainerProperties() },
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
			else if (propertyName.SequenceEqual("fontSize"))
			{
				labeledObject.FontSize = int.Parse(value);
			}
			else if (propertyName.SequenceEqual("fontFamily"))
			{
				labeledObject.FontFamily = value.ToString();
			}
		}

		if (steffiObject is IChildObject childObject && childObject.ParentProperties is CanvasContainerProperties canvasContainerProperties)
		{
			if (propertyName.SequenceEqual("x"))
			{
				canvasContainerProperties.X = int.Parse(value);
			}
			else if (propertyName.SequenceEqual("y"))
			{
				canvasContainerProperties.Y = int.Parse(value);
			}
		}

		if (steffiObject is Rectangle rectangle)
		{
			if (propertyName.SequenceEqual("width"))
			{
				rectangle.Width = int.Parse(value);
			}
			else if (propertyName.SequenceEqual("height"))
			{
				rectangle.Height = int.Parse(value);
			}
		}
	}
}