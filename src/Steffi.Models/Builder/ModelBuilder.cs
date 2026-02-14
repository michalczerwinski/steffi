using Steffi.Models.Containers;
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

	public static bool SetObjectProperty(SteffiObject steffiObject, ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
	{
		if (steffiObject is ILabeledObject labeledObject)
		{
			if (propertyName.SequenceEqual("label"))
			{
				labeledObject.Label = value.ToString();
				return true;
			}
			else if (propertyName.SequenceEqual("fontColor"))
			{
				labeledObject.FontColor = value.ToString();
				return true;
			}
			else if (propertyName.SequenceEqual("fontSize"))
			{
				labeledObject.FontSize = int.Parse(value);
				return true;
			}
			else if (propertyName.SequenceEqual("fontFamily"))
			{
				labeledObject.FontFamily = value.ToString();
				return true;
			}
		}

		if (steffiObject is IChildObject childObject && childObject.ParentProperties is CanvasContainerProperties canvasContainerProperties)
		{
			if (propertyName.SequenceEqual("x"))
			{
				canvasContainerProperties.X = int.Parse(value);
				return true;
			}
			else if (propertyName.SequenceEqual("y"))
			{
				canvasContainerProperties.Y = int.Parse(value);
				return true;
			}
		}

		if (steffiObject is Rectangle rectangle)
		{
			if (propertyName.SequenceEqual("width"))
			{
				rectangle.Width = int.Parse(value);
				return true;
			}
			else if (propertyName.SequenceEqual("height"))
			{
				rectangle.Height = int.Parse(value);
				return true;
			}
		}
		return false;
	}
}