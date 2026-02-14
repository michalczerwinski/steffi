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
		if (steffiObject is Text text)
		{

			if (propertyName.Equals(nameof(Text.Spans), StringComparison.InvariantCultureIgnoreCase))
			{
				text.Spans = value.ToString();
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontColor), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontColor = value.ToString();
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontSize), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontSize = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(Text.FontFamily), StringComparison.InvariantCultureIgnoreCase))
			{
				text.FontFamily = value.ToString();
				return true;
			}
		}

		if (steffiObject is IChildObject childObject && childObject.ParentProperties is CanvasContainerProperties canvasContainerProperties)
		{
			if (propertyName.Equals(nameof(CanvasContainerProperties.X), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.X = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(CanvasContainerProperties.Y), StringComparison.InvariantCultureIgnoreCase))
			{
				canvasContainerProperties.Y = int.Parse(value);
				return true;
			}
		}

		if (steffiObject is Rectangle rectangle)
		{
			if (propertyName.Equals(nameof(Rectangle.Width), StringComparison.InvariantCultureIgnoreCase))
			{
				rectangle.Width = int.Parse(value);
				return true;
			}
			else if (propertyName.Equals(nameof(Rectangle.Height), StringComparison.InvariantCultureIgnoreCase))
			{
				rectangle.Height = int.Parse(value);
				return true;
			}
		}
		return false;
	}
}