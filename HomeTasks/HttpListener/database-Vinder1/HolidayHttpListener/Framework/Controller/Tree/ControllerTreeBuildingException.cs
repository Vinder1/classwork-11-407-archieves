using System.Reflection;

namespace HolidayHttpListener.Framework.Controller.Tree;

public class ControllerTreeBuildingException : Exception
{
    public ControllerTreeBuildingException(string message) : base(message) { }
    
    public ControllerTreeBuildingException(MethodInfo method, string message)
        : base($"[{method.DeclaringType}.{method.Name}] {message}") {}
}
