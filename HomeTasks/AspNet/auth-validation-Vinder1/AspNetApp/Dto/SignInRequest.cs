namespace AspNetApp.Dto;

/// <summary>
/// Represents a request for signing up or registering a new player account.
/// </summary>
public class SignInRequest
{
    /// <summary>
    /// The display name of the new player.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The login for authentication.
    /// </summary>
    public string Login { get; init; } = null!;

    /// <summary>
    /// The password for the new account.
    /// </summary>
    public string Password { get; init; } = null!;
}