using AspNetApp.Auth;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;

namespace AspNetApp.Services;

/// <summary>
/// Implementation of the interface
/// </summary>
/// <param name="playerRepository"></param>
/// <param name="jwtService"></param>
/// <param name="cookieService"></param>
public class AuthService(IPlayerRepository playerRepository, IJwtService jwtService, ICookieService cookieService) : IAuthService
{
    private readonly IPlayerRepository _playerRepository = playerRepository
        ?? throw new ArgumentNullException(nameof(playerRepository));
    private readonly IJwtService _jwtService = jwtService
        ?? throw new ArgumentNullException(nameof(jwtService));
    private readonly ICookieService _cookieService = cookieService
        ?? throw new ArgumentNullException(nameof(cookieService));

    private Player? user;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string?> LogInAsync(string login, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (login.Length < 3 || login.Length > 50)
        {
            throw new ArgumentException("Player login must be between 3 and 50 characters");
        }

        var user = await _playerRepository.GetByLoginAsync(login);

        if (user == null || !PasswordHashBuilder.Validate(user.PasswordHash, password))
        {
            return null;
        }

        this.user = user;

        return _jwtService.GenerateToken(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task SaveUserToCookie(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(user);
        return _cookieService.SignIn(context, user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string> SignInNewPlayerAsync(string name, string login, string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(login);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        if (name.Length < 3 || name.Length > 50)
        {
            throw new ArgumentException("Player name must be between 3 and 50 characters");
        }
        
        if (login.Length < 3 || login.Length > 50)
        {
            throw new ArgumentException("Player login must be between 3 and 50 characters");
        }

        var guid = Guid.NewGuid();
        var passwordHash = PasswordHashBuilder.GetHash(password);

        var user = await _playerRepository.CreateAsync(guid, new Player
        {
            Id = guid,
            Name = name,
            Login = login,
            Role = "User",
            PasswordHash = passwordHash
        });

        this.user = user;

        return _jwtService.GenerateToken(user);
    }
}