using AspNetApp.Auth;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;
using AutoMapper;

namespace AspNetApp.Services;

/// <summary>
/// Implementation of the interface
/// </summary>
/// <param name="playerRepository"></param>
/// <param name="jwtService"></param>
/// <param name="cookieService"></param>
/// <param name="logger"></param>
/// <param name="mapper"></param>
public class AuthService(
    IPlayerRepository playerRepository,
    IJwtService jwtService,
    ICookieService cookieService,
    ILogger<AuthService> logger,
    IMapper mapper) : IAuthService
{
    private readonly IPlayerRepository _playerRepository = playerRepository
        ?? throw new ArgumentNullException(nameof(playerRepository));
    private readonly IJwtService _jwtService = jwtService
        ?? throw new ArgumentNullException(nameof(jwtService));
    private readonly ICookieService _cookieService = cookieService
        ?? throw new ArgumentNullException(nameof(cookieService));
    private readonly ILogger<AuthService> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMapper _mapper = mapper
        ?? throw new ArgumentNullException(nameof(mapper));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<UserDto?> LogInAsync(string login, string password)
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

        _logger.LogInformation("Successful log in for user with login={Login}", login);

        return _mapper.Map<UserDto>(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string GetJwtTokenForUser(UserDto user)
    {
       return _jwtService.GenerateToken(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public Task SaveUserToCookie(UserDto user, HttpContext context)
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
    public async Task<UserDto> SignInNewPlayerAsync(string name, string login, string password)
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

        var player = await _playerRepository.CreateAsync(guid, new Player
        {
            Id = guid,
            Name = name,
            Login = login,
            Role = "User",
            PasswordHash = passwordHash
        });

        _logger.LogInformation("Successfully created new user with login={Login}", login);

        return _mapper.Map<UserDto>(player);
    }
}