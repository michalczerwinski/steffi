namespace Steffi.Parsers.Model;

public interface IParentObject
{
	List<SteffiObject> Children { get; }
}

public interface INamedObject
{
	string Name { get; }
}
