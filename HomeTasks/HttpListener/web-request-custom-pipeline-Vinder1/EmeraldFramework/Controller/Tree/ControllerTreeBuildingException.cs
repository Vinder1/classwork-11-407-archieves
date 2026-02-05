using System.Reflection;

namespace Emerald.Controller.Tree;

public class ControllerTreeBuildingException : Exception
{
    public ControllerTreeBuildingException(string message) : base(message) { }
    
    public ControllerTreeBuildingException(MethodInfo method, string message)
        : base($"[{method.DeclaringType}.{method.Name}] {message}") {}
}
