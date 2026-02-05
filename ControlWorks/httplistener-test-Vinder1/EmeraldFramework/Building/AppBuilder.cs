using Emerald.Controller.Tree;
using Emerald.Exceptions;
using Emerald.Services;
using Emerald.Middleware;

namespace Emerald.Building;

public class AppBuilder
{
    public ControllerTree Controllers { get; } = new();
    public ServiceCollection Services { get; } = new();
    public LocalFileProvider? LocalFileProvider { get; set; }

    public MiddlewarePipeline Pipeline { get; } = new();
    public IExceptionHandler? ExceptionHandler { get; set; }

    public RequestHandler Build()
    {
        if (LocalFileProvider != null)
            Services.AddInstance(LocalFileProvider);
        Controllers.Build(Services);
        Pipeline.Add(new EndPoint(Controllers));
        return new RequestHandler()
        {
            Pipeline = Pipeline,
            LocalFileProvider = LocalFileProvider,
        };
    }
}
