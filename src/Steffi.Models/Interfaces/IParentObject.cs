namespace Steffi.Models.Interfaces;

public interface IParentObject
{
	LayoutType Layout { get; set; }

	List<SteffiObject> Children { get; }
}
