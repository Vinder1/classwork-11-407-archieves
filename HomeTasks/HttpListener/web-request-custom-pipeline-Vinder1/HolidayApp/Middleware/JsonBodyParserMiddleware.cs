using Emerald;
using Emerald.Exceptions;
using Emerald.Middleware;

namespace HolidayApp.Middleware;

public class JsonBodyParserMiddleware : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        if (context.EndPoint == null)
        {
            throw new PipelineBuildingException("JsonBodyParser expects endpoint to be already defined");
        }

        var type = context.EndPoint.RequiredModelType;
        if (type == null)
        {
            await MoveNext(context);
            return;
        }

        var model = await context.ReadJson(type, context.CancellationToken);
        if (model == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        context.Model = model;

        await MoveNext(context);
    }
    
    private async Task MoveNext(Emerald.AppContext context)
    {
        if (Next == null)
            return;
        await Next.InvokeAsync(context);
    }
}
