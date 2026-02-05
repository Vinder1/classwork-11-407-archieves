using Emerald.Controller.Tree;
using Emerald.Exceptions;
using Emerald.Services;
using Emerald.Middleware;
using Emerald.Services.Logging;

namespace Emerald.Building;

public class AppBuilder
{
    public ControllerTree Controllers { get; } = new();
    public ServiceCollection Services { get; } = new();
    public StaticFileProvider? LocalFileProvider { get; set; }

    public MiddlewarePipeline Pipeline { get; } = new();
    public IExceptionHandler? ExceptionHandler { get; set; }

    public RequestHandler Build()
    {
        Controllers.Build(Services);
        if (!Pipeline.Contains<EndPointFindMiddleware>())
        {
            Pipeline.Add(new EndPointFindMiddleware(Controllers));
        }
        Pipeline.Add(new ExecuteMiddleware());

        ILogger? logger = null;
        try
        {
            logger = Services.GetSingleton<ILogger>();
        } catch {} 

        return new RequestHandler()
        {
            Pipeline = Pipeline,
            LocalFileProvider = LocalFileProvider,
            Logger = logger,
        };
    }
}
