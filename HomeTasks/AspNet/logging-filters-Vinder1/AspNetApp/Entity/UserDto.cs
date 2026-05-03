namespace AspNetApp.Entity;

/// <summary>
/// Represents a user
/// </summary>
public class UserDto
{
    /// <summary>
    /// The unique identifier of the player.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The display name of the player.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The login username for authentication.
    /// </summary>
    public string Login { get; init; } = null!;

    /// <summary>
    /// The role assigned to the player.
    /// </summary>
    public string Role { get; init; } = null!;
}