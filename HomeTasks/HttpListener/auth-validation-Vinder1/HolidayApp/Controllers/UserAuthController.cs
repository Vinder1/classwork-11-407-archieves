using Emerald;
using Emerald.Controller;
using Emerald.Exceptions;
using HolidayApp.Repositories;
using HolidayApp.Services;

namespace HolidayApp.Controllers;

[Controller]
public class UserAuthController(UserRepository userRepository, JwtAuth jwtAuth)
{
    private UserRepository Repository => userRepository;
    private JwtAuth Auth => jwtAuth;

    [Controller.Get("api/getLocalUser")]
    public async Task GetUserByToken(Emerald.AppContext context)
    {
        var res = Auth.ValidateJwtToken(context.GetTokenFromHeader());
        if (!res.isSuccess || res.id == null)
        {
            context.SetResponseStatusCode(StatusCode.Unauthorized401);
            return;
        }

        var user = await Repository.GetPublicInformationByIdAsync(res.id.Value, context.CancellationToken);
        if (user == null)
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }
        else
        {
            context.SetResponseStatusCode(StatusCode.OK);
            await context.WriteJson(new { user });
        }
    }

    [Controller.Get("api/tryGetNewToken")]
    public async Task TryGetNewToken(Emerald.AppContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken?.Value))
        {
            context.SetResponseStatusCode(StatusCode.BadRequest400);
            return;
        }

        var result = Auth.ValidateRefreshToken(refreshToken.Value);
        if (result.isSuccess)
        {
            var newAccessToken = Auth.GenerateJwtToken(result.id!.Value);
            context.SetResponseStatusCode(StatusCode.OK);
            await context.WriteJson(new { token = newAccessToken });
        }
    }
}
