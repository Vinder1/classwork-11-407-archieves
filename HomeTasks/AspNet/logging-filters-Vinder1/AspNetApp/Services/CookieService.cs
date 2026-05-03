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
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task SignIn(HttpContext context, UserDto user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Login),
            new (ClaimTypes.Role, user.Role)
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