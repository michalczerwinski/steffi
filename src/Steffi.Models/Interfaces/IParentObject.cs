namespace Steffi.Models.Interfaces;

public interface IParentObject
{
	LayoutType Layout { get; set; }

	List<SteffiObject> Children { get; }

	ParentContainerProperties CreateContainerProperties();
}

public abstract class ParentContainerProperties;

public class EmptyContainerProperties : ParentContainerProperties
{
}

public class CanvasContainerProperties : ParentContainerProperties
{
	public int X { get; set; }
	public int Y { get; set; }
}


public interface IChildObject
{
	IParentObject Parent { get; set; }

	ParentContainerProperties ParentProperties { get; set; }
}
