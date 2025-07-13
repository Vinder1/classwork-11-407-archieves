namespace Reflextion;

public static class DependencyInjector
{
    public static void Register<TInterface, TImplementation>()
        => Register(typeof(TInterface), typeof(TImplementation));
    public static T Resolve<T>()
    {
        if (typeof(T).IsAbstract)
            return (T)CreateInstance(dependencies[typeof(T)]);
        return (T)CreateInstance(typeof(T));
    }
    
    private static void Register(Type interfaceType, Type implementation)
    {
        if (!interfaceType.IsAbstract)
            throw new Exception("Input type must be abstract.");
        if (implementation.IsAbstract)
            throw new Exception("Output mustnt be abstract.");
        dependencies[interfaceType] = implementation;
    }

    private static object CreateInstance(Type type)
    {
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
                    parameters[i] = CreateInstance(paramType);
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