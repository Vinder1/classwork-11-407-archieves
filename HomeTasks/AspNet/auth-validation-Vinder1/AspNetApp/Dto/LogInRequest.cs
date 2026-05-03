namespace AspNetApp.Dto;

/// <summary>
/// Represents a request for user authentication with login credentials.
/// </summary>
public class LogInRequest
{
    /// <summary>
    /// Login for authentication.
    /// </summary>
    public string Login { get; init; } = null!;

    /// <summary>
    /// Password for authentication.
    /// </summary>
    public string Password { get; init; } = null!;
}