namespace Emerald.Controller;

/// <summary>
/// Its name doesn`t end with 'Attribute' for easier access to internal attributes
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class Controller : Attribute
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GetAttribute(string Path) : HttpAttribute(Path, HttpMethod.GET) { }

    [AttributeUsage(AttributeTargets.Method)]
    public class PostAttribute(string Path) : HttpAttribute(Path, HttpMethod.POST) { }

    [AttributeUsage(AttributeTargets.Method)]
    public class PutAttribute(string Path) : HttpAttribute(Path, HttpMethod.PUT) { }

    [AttributeUsage(AttributeTargets.Method)]
    public class DeleteAttribute(string Path) : HttpAttribute(Path, HttpMethod.DELETE) { }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class PatchAttribute(string Path) : HttpAttribute(Path, HttpMethod.PATCH) {}
    //public string Route { get; } = route;
}
