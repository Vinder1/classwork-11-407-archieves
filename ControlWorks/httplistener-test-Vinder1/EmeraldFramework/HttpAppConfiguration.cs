using System.Reflection;
using Emerald.Exceptions;
using Emerald.Middleware;

namespace Emerald;

public partial class HttpApp
{
    /// <summary>
    /// Provides a path to the directory where static files are located.
    /// Files are loaded recursively from every subdirectory.
    /// Currently, only single directory with files is supported.
    /// </summary>
    /// <param name="path">Relative path to directory</param>
    public void UseStaticFiles(string path)
    {
        builder.LocalFileProvider = new (path);
    }

    /// <summary>
    /// Use <see cref="TExceptionHandler"> as Exception Handler
    /// </summary>
    public void UseExceptionHandler<TExceptionHandler>()
    {
        builder.ExceptionHandler = Activator.CreateInstance<TExceptionHandler>() as IExceptionHandler;
    }

    /// <summary>
    /// Add controllers from given assembly
    /// </summary>
    /// <param name="from"></param>
    public void AddControllersFromAssembly(Assembly from)
    {
        builder.Controllers.AddControllers(from);
    }

    /// <summary>
    /// Add configuration service. This service is recommended as source of all configuration strings, used in other 
    /// services constructors.
    /// </summary>
    /// <param name="configFilePath"> name of config file and path to it </param>
    public void AddServiceConfigurator(string configFilePath)
    {
        AddSingletonInstance(new Configuration(configFilePath));
    }

    /// <summary>
    /// Register new singleton service
    /// </summary>
    public void RegisterService<TInterface, TService>()
    {
        builder.Services.Register<TInterface, TService>();
    }

    /// <summary>
    /// Add existing singleton object. No lazy initialization? Da mne do lampy, i tak po kaifu
    /// </summary>
    public void AddSingletonInstance<TInterface>(TInterface service)
    {
        builder.Services.AddInstance(service);
    }

    /// <summary>
    /// Add middleware of given type to the current end of pipeline. Can resolve services dependencies.
    /// </summary>
    public void AddMiddleware<TMiddleware>() where TMiddleware : IMiddleware
    {
        var singletonMiddleware = builder.Services.GetSingleton<TMiddleware>();
        builder.Pipeline.Add(singletonMiddleware);
    }
}
