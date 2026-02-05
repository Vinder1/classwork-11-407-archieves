using System.Reflection;
using Emerald.Exceptions;
using Emerald.Middleware;
using Emerald.Services.Logging;
using Emerald.Validation;

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
        if (builder.LocalFileProvider != null)
        {
            Console.WriteLine("Multiple directories are not supported");
            return;
        }
        builder.LocalFileProvider = new(path);
        builder.Services.AddInstance(builder.LocalFileProvider);
    }

    /// <summary>
    /// Use <see cref="TExceptionHandler"> as Exception Handler
    /// </summary>
    public void UseExceptionHandler<TExceptionHandler>()
    {
        builder.ExceptionHandler = builder.Services.GetSingleton<TExceptionHandler>() as IExceptionHandler;
    }
    
    /// <summary>
    /// Register logger type
    /// </summary>
    public void UseLogger<TLogger>() where TLogger : ILogger
    {
        builder.Services.Register<ILogger, TLogger>();
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

    /// <summary>
    /// Add middleware that will determine static files and send them
    /// </summary>
    public void AddStaticFilesMiddleware()
    {
        AddMiddleware<StaticFilesMiddleware>();
    }

    /// <summary>
    /// Add middleware that will find controller endpoint for this request
    /// </summary>
    public void AddEndPointFindMiddleware()
    {
        builder.Pipeline.Add(new EndPointFindMiddleware(builder.Controllers));
    }

    /// <summary>
    /// Register Validator for given type. TValidator needs to implement IValidator interface
    /// </summary>
    public void AddValidator<TModel, TValidator>() where TValidator : IValidator<TModel>
    {
        var validatorMap = builder.Services.GetSingleton<ValidatorMap>();
        validatorMap.Register<TModel, TValidator>(builder.Services);
    }
}
