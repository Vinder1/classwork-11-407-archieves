using System.Reflection;
using Emerald.Exceptions;

namespace Emerald.Services;

/// <summary>
/// Basically, Dependency Injector.
/// Only returns singletons, because I LOVE SINGLETONS
/// </summary>
public class ServiceCollection
{
    public void Register<TInterface, TImplementation>()
        => Register(typeof(TInterface), typeof(TImplementation));

    private void Register(Type interfaceType, Type implementation)
    {
        if (!interfaceType.IsAbstract)
            throw new Exception("Input type must be abstract.");
        if (implementation.IsAbstract)
            throw new Exception("Output must not be abstract.");
        dependencies[interfaceType] = implementation;
    }

    public void AddInstance<TInterface>(TInterface t)
    {
        if (t == null)
            throw new Exception("t must be not null");
        singletons.Add(typeof(TInterface), t);
    }

    public T GetSingleton<T>()
        => (T)GetSingleton(typeof(T));

    public object GetSingleton(Type t)
    {
        t = GetProperType(t);
        if (singletons.TryGetValue(t, out var service))
            return service;
        var res = CreateInstance(t);
        singletons.Add(t, res);
        return res;
    }

    private Type GetProperType(Type type)
        => type.IsAbstract ? dependencies[type] : type;

    private object CreateInstance(Type type)
    {
        // I deleted type registration check, because why not

        var ctors = type.GetConstructors();
        var noParamsCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
        if (noParamsCtor != null)
        {
            return noParamsCtor.Invoke(null);
        }

        foreach (var ctor in ctors)
        {
            try
            {
                var parametersTypes = ctor.GetParameters();
                var parameters = new object[parametersTypes.Length];
                for (var i = 0; i < parametersTypes.Length; i++)
                {
                    var paramType = parametersTypes[i].ParameterType;
                    parameters[i] = GetSingleton(paramType);
                }

                return ctor.Invoke(parameters);
            }
            catch (TargetInvocationException T)
            {
                if (T.InnerException is InitializationException e)
                {
                    throw e;
                }
                // Try another ctor
            }
            catch
            {
                throw;
            }
        }

        throw new Exception("No proper constructor found.");
    }

    private readonly Dictionary<Type, Type> dependencies = [];
    private readonly Dictionary<Type, object> singletons = [];
}
