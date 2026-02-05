using System.Reflection;
using Emerald.Exceptions;
using Emerald.Services;

namespace Emerald.Controller.Tree;

public class ControllerTree
{
    private readonly ControllerTreeNode root = new() { Type = ResourceType.None };
    private readonly List<Assembly> assemblies = [];

    public void AddControllers(Assembly from)
    {
        assemblies.Add(from);
    }

    public void Build(ServiceCollection services)
    {
        foreach (var assembly in assemblies)
        {
            BuildFromAssembly(services, assembly);
        }
        assemblies.Clear();
    }

    private void BuildFromAssembly(ServiceCollection services, Assembly from)
    {
        var controllers = from.GetTypes()
            .Where(t => t.CustomAttributes
                .Any(a => typeof(Controller).IsAssignableFrom(a.AttributeType)));

        foreach (var controller in controllers)
        {
            var controllerObject = services.GetSingleton(controller);
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