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
    Task<string> SignInNewPlayerAsync(string name, string login, string password);

    /// <summary>
    /// Find user
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns> Jwt token for user </returns>
    Task<string?> LogInAsync(string login, string password);

    /// <summary>
    /// Creates cookies for this user; Can`t be called before methods above 
    /// </summary>
    /// <returns></returns>
    Task SaveUserToCookie(HttpContext context);
}