using System.Security.Claims;
using AspNetApp.Entity;
using AspNetApp.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNetApp.Services;

/// <summary>
/// 
/// </summary>
public class CookieService : ICookieService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="player"></param>
    /// <returns></returns>
    public async Task SignIn(HttpContext context, Player player)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, player.Id.ToString()),
            new (ClaimTypes.Name, player.Login),
            new (ClaimTypes.Role, player.Role)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal,
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}