using System.Net;
using System.Reflection;
using HolidayHttpListener.Framework.Exceptions;

namespace HolidayHttpListener.Framework.Controller.Tree;

public class ControllerTree
{
    private readonly ControllerTreeNode root = new() { Type = ResourceType.None };
    public void Build()
    {
        var controllers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.CustomAttributes.Any(a => typeof(Controller).IsAssignableFrom(a.AttributeType)));

        foreach (var controller in controllers)
        {
            var controllerObject = Activator.CreateInstance(controller)!;
            foreach (var method in controller.GetMethods()
                .Where(m => m.CustomAttributes.Any(a => typeof(HttpAttribute).IsAssignableFrom(a.AttributeType)))
            )
            {
                if (method.ReturnParameter.ParameterType != typeof(Task))
                {
                    throw new ControllerTreeBuildingException(method, "Incorrect signature: method return type must be Task");
                }
                if (method.GetParameters().Length == 0 || method.GetParameters()[0].ParameterType != typeof(AppContext))
                {
                    throw new ControllerTreeBuildingException(method, "Incorrect signature: first parameter must be with 'AppContext' type");
                }
                root.Add(method, controllerObject);
            }
        }
    }

    public async Task Invoke(AppContext context, string path)
    {
        if (!await root.TryInvoke(context, new ArgumentArrayBuilder(path, context)))
        {
            throw new HttpException(StatusCode.NotFound404);
        }
    }
}