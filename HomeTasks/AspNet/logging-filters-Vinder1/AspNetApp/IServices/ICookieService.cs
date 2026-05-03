using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// Service for handling user authentication cookies and sign-in operations.
/// </summary>
public interface ICookieService
{
    /// <summary>
    /// Signs in a player by creating an authentication cookie.
    /// </summary>
    /// <param name="context">The HTTP context of the current request.</param>
    /// <param name="user">The player entity to sign in.</param>
    Task SignIn(HttpContext context, UserDto user);
}