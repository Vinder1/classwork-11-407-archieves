using Emerald;
using Emerald.Exceptions;
using Emerald.Middleware;
namespace HolidayApp.Middleware;

public class AuthorizationMiddleware : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        if (context.EndPoint == null)
        {
            throw new PipelineBuildingException("JsonBodyParser expects endpoint to be already defined");
        }

        if (NeedToInterruptUnauthorized(context, out var statusCode))
        {
            if (context.EndPoint.Redirect != null)
                context.Response.Redirect(context.EndPoint.Redirect);
            else
                context.SetResponseStatusCode(statusCode);
            return;
        }

        await MoveNext(context);
    }

    private bool NeedToInterruptUnauthorized(Emerald.AppContext context, out StatusCode statusCode)
    {
        var auth = context.AuthInfo;
        var roleRequired = context.EndPoint.RoleRequired;
        statusCode = StatusCode.Unknown;

        if (roleRequired == null) // everybody can visit this page, nothing required
        {
            return false;
        }

        if (auth == null) // not authorized, but any non-null role required
        {
            statusCode = StatusCode.Unauthorized401;
            return true;
        }

        if (roleRequired == "any") // user must have any role, whichever it is. Its already checked
        {
            return false;
        }

        if (roleRequired != auth.Role)
        {
            statusCode = StatusCode.Forbidden403;
            return true;
        }
        return false;
    }

    private async Task MoveNext(Emerald.AppContext context)
    {
        if (Next == null)
            return;
        await Next.InvokeAsync(context);
    }
}
