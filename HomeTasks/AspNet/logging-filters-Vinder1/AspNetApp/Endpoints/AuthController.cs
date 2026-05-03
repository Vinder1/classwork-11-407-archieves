using AspNetApp.Dto;
using AspNetApp.IServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Endpoints;

/// <summary>
/// 
/// </summary>
/// <param name="authService"></param>
/// <param name="logger"></param>
[ApiController]
[Route("[controller]")]
[Tags("Auth")]
public class AuthController(
    IAuthService authService,
    ILogger<AuthController> logger
) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="playerService"></param>
    /// <param name="validator"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> SignIn(
        [FromServices] IPlayerService playerService,
        IValidator<SignInRequest> validator,
        [FromBody] SignInRequest request)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
            if (await playerService.PlayerLoginExistsAsync(request.Login))
            {
                return Results.BadRequest(new
                {
                    error = "Пользователь с таким логином уже существует"
                });
            }

            _logger.LogInformation("Attempt to sign up from {@Request}", request);

            var user = await _authService.SignInNewPlayerAsync(
                name: request.Name, login: request.Login, password: request.Password);

            var token = _authService.GetJwtTokenForUser(user);

            await _authService.SaveUserToCookie(user, HttpContext);

            return Results.Ok(new
            {
                token
            });
        }
        catch
        {
            return Results.InternalServerError();
        }
    }

    /// <summary>
    /// Вход существующего пользователя
    /// </summary>
    [HttpPost("log-in")]
    public async Task<IResult> LogIn(
        IValidator<LogInRequest> validator,
        [FromBody] LogInRequest request)
    {
        try
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            _logger.LogInformation("Attempt to log in from {@Request}", request);

            var user = await _authService.LogInAsync(
                login: request.Login,
                password: request.Password);

            if (user == null)
            {
                return Results.Unauthorized();
            }

            var token = _authService.GetJwtTokenForUser(user);

            await _authService.SaveUserToCookie(user, HttpContext);

            return Results.Ok(new
            {
                token
            });
        }
        catch
        {
            return Results.InternalServerError();
        }
    }
}