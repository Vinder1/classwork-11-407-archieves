using AspNetApp.Dto;
using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// 
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Create new player
    /// </summary>
    /// <param name="name"></param>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns> Jwt token for user </returns>
    Task<UserDto> SignInNewPlayerAsync(string name, string login, string password);

    /// <summary>
    /// Find user
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns> Jwt token for user </returns>
    Task<UserDto?> LogInAsync(string login, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GetJwtTokenForUser(UserDto user);

    /// <summary>
    /// Creates cookies for this user
    /// </summary>
    /// <returns></returns>
    Task SaveUserToCookie(UserDto user, HttpContext context);
}