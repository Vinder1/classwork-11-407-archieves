namespace Reflextion;

public static class DependencyInjector
{
    public static void Register<TInterface, TImplementation>()
        => Register(typeof(TInterface), typeof(TImplementation));
    
    public static T Resolve<T>()
    {
        return (T)CreateInstance(GetProperType(typeof(T)));
    }
    
    private static void Register(Type interfaceType, Type implementation)
    {
        if (!interfaceType.IsAbstract)
            throw new Exception("Input type must be abstract.");
        if (implementation.IsAbstract)
            throw new Exception("Output must not be abstract.");
        dependencies[interfaceType] = implementation;
    }
    
    private static Type GetProperType(Type type)
    {
        return type.IsAbstract ? dependencies[type] : type;
    }

    private static object CreateInstance(Type type)
    {
        if (!dependencies.ContainsValue(type))
        {
            throw new Exception($"Type {type.FullName} is not registered.");
        }
        
        var ctors = type.GetConstructors();
        var noParamsCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
        if (noParamsCtor != null)
            return noParamsCtor.Invoke(null);

        foreach (var ctor in ctors)
        {
            try
            {
                var parametersTypes = ctor.GetParameters();
                var parameters = new object[parametersTypes.Length];
                for (var i = 0; i < parametersTypes.Length; i++)
                {
                    var paramType = parametersTypes[i].ParameterType;
                    parameters[i] = CreateInstance(GetProperType(paramType));
                }

                return ctor.Invoke(parameters);
            }
            catch
            {
                // Try another ctor
            }
        }

        throw new Exception("No proper constructor found.");
    }

    private static Dictionary<Type, Type> dependencies = new();
}