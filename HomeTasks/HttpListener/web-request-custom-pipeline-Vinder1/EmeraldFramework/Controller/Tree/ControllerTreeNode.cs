using System.Net;
using System.Reflection;
using EmeraldFramework.Controller;
using EmeraldFramework.Controller.Tree;

namespace Emerald.Controller.Tree;

public class ControllerTreeNode
{
    public required ResourceType Type { get; init; }
    public string? Pattern { get; init; }
    private readonly List<ControllerTrigger> Triggers = [];

    private record TriggerMetadata(string? RoleRequired = null, string? Redirect = null, Type? RequiredModelType = null);

    private record ControllerTrigger(
        object Controller,
        MethodInfo Method,
        HttpMethod HttpMethod,
        TriggerMetadata Meta,
        string? Pattern = null)
    {
        public Task Invoke(object[] args)
        {
            return (Task)Method.Invoke(Controller, args)!;
        }
    }

    private readonly List<ControllerTreeNode> Const = [];
    private ControllerTreeNode? OnInt = null;
    private ControllerTreeNode? OnString = null;


    public void Add(MethodInfo controllerMethod, object controller)
    {
        var attribute = controllerMethod
            .GetCustomAttributes(typeof(HttpAttribute), inherit: true)
            .OfType<HttpAttribute>().SingleOrDefault()
            ?? throw new ControllerTreeBuildingException(controllerMethod, "Attribute missing");

        var path = attribute.Path;
        var method = attribute.Method;

        if (path == null)
            throw new ControllerTreeBuildingException(controllerMethod, "Path undefined");

        var resources = path.Split('/').Where(r => r != "").ToArray();
        var currentNode = this;
        List<Type> requiredArgumentTypes = [typeof(AppContext)];

        foreach (var resource in resources)
        {
            if (resource[0] != '{')
            {
                var nextNode = currentNode.Const.SingleOrDefault(t => t.Pattern == resource);
                if (nextNode == null)
                {
                    nextNode = new ControllerTreeNode()
                    {
                        Type = ResourceType.Const,
                        Pattern = resource
                    };
                    currentNode.Const.Add(nextNode);
                }
                currentNode = nextNode;
            }

            else if (resource == "{int}")
            {
                currentNode.OnInt ??= new ControllerTreeNode
                {
                    Type = ResourceType.IntPattern
                };
                currentNode = currentNode.OnInt;
                requiredArgumentTypes.Add(typeof(int));
            }

            else if (resource == "{str}")
            {
                currentNode.OnString ??= new ControllerTreeNode
                {
                    Type = ResourceType.StringPattern
                };
                currentNode = currentNode.OnString;
                requiredArgumentTypes.Add(typeof(string));
            }
            else
                throw new ControllerTreeBuildingException(controllerMethod, "Invalid method type: unrecognized pattern type");
        }

        var args = controllerMethod.GetParameters();
        if (args.Length != requiredArgumentTypes.Count)
        {
            throw new ControllerTreeBuildingException(controllerMethod,
                $"Invalid parameter count: {args.Length} instead of {requiredArgumentTypes.Count}");
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].ParameterType != requiredArgumentTypes[i])
            {
                throw new ControllerTreeBuildingException(controllerMethod,
                    $"Invalid {i} parameter type: {args[i].ParameterType} instead of {requiredArgumentTypes[i]}");
            }
        }

        if (currentNode.Triggers.Any(t => t.HttpMethod == method))
        {
            throw new ControllerTreeBuildingException(controllerMethod,
                $"Duplicate triggers handling method {method} detected");
        }

        var authorize = controllerMethod
            .GetCustomAttributes(typeof(AuthorizedAttribute))
            .OfType<AuthorizedAttribute>().SingleOrDefault();

        var requireModel = controllerMethod
            .GetCustomAttributes()
            .FirstOrDefault(attr =>
                attr.GetType().IsGenericType &&
                attr.GetType().GetGenericTypeDefinition() == typeof(RequireModelAttribute<>));

        currentNode.Triggers.Add(new(
            Controller: controller,
            Method: controllerMethod,
            HttpMethod: method,
            Meta: new(
                RoleRequired: authorize?.Role,
                Redirect: authorize?.Redirect,
                RequiredModelType: requireModel?.GetType().GetGenericArguments()[0]
            )
        ));
    }

    public ControllerEndPoint? Find(AppContext context, ArgumentArrayBuilder arguments)
    {
        if (!TryFindEndPointNode(arguments, out var node))
            return null;

        var method = context.GetHttpMethod();
        var methodTrigger = node!.Triggers.SingleOrDefault(x => x.HttpMethod == method);
        if (methodTrigger != null)
        {
            return new ControllerEndPoint
            {
                Action = () => methodTrigger.Invoke(arguments.Build()),
                RoleRequired = methodTrigger.Meta.RoleRequired,
                Redirect = methodTrigger.Meta.Redirect,
                RequiredModelType = methodTrigger.Meta.RequiredModelType
            };
        }
        return null;
    }

    private bool TryFindEndPointNode(ArgumentArrayBuilder arguments, out ControllerTreeNode? endpoint)
    {
        var node = this;
        while (!arguments.End)
        {
            ControllerTreeNode? OnConst = node!.Const.SingleOrDefault(t => t.Pattern == arguments.Current);
            if (OnConst != null)
            {
                arguments.MoveNext(ResourceType.Const);
                node = OnConst;
                continue;
            }

            if (node.OnInt != null && int.TryParse(arguments.Current, out var _))
            {
                arguments.MoveNext(ResourceType.IntPattern);
                node = node.OnInt;
                continue;
            }

            if (node.OnString != null)
            {
                arguments.MoveNext(ResourceType.StringPattern);
                node = node.OnString;
                continue;
            }
            endpoint = null;
            return false;
        }
        endpoint = node;
        return true;
    }
}