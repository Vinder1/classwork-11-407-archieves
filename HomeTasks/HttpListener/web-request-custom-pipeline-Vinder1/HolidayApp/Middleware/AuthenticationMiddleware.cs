using Emerald;
using Emerald.Middleware;
using HolidayApp.Services;

namespace HolidayApp.Middleware;

public class AuthenticationMiddleware(JwtAuth Auth) : IMiddleware
{
    public IMiddleware? Next { set; get; }

    public async Task InvokeAsync(Emerald.AppContext context)
    {
        TryGetUserData(context);
        if (Next == null)
            return;
        await Next.InvokeAsync(context);
    }

    private void TryGetUserData(Emerald.AppContext context)
    {
        var res = Auth.ValidateJwtToken(context.GetTokenFromHeader());
        if (res != null)
        {
            context.AuthInfo = res;
            return;
        }
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (refreshToken == null || string.IsNullOrEmpty(refreshToken.Value))
        {
            return;
        }

        var info = Auth.ValidateRefreshToken(refreshToken.Value);
        if (info != null)
        {
            context.AuthInfo = info;
            return;
        }
    }
}
