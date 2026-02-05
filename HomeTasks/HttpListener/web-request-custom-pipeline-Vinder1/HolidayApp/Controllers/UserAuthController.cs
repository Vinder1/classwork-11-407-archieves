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
        var info = context.AuthInfo;
        if (info == null)
        {
            var token = GenerateNewToken(context);

            if (token != null)
            {
                info = Auth.ValidateJwtToken(token);
                context.Response.Headers["X-New-Token"] = token;
            }
            else
            {
                context.SetResponseStatusCode(StatusCode.Unauthorized401);
                return;
            }
        }

        var user = await Repository.GetPublicInformationByIdAsync(info!.Id, context.CancellationToken);
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

    private string? GenerateNewToken(Emerald.AppContext context)
    {
        var refreshToken = context.Request.Cookies["refreshToken"];

        if (refreshToken == null || string.IsNullOrEmpty(refreshToken?.Value))
        {
            return null;
        }

        var info = Auth.ValidateRefreshToken(refreshToken.Value);
        if (info == null)
        {
            return null;
        }

        return Auth.GenerateJwtToken(info);
    }
}
