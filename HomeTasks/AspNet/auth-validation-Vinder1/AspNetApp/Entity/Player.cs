namespace AspNetApp.Entity;

/// <summary>
/// Represents a player entity with authentication data and spaceship reference.
/// </summary>
public class Player
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
    /// The hashed password for security.
    /// </summary>
    public string PasswordHash { get; init; } = null!;

    /// <summary>
    /// The role assigned to the player.
    /// </summary>
    public string Role { get; init; } = null!;

    /// <summary>
    /// The timestamp when the player was registered.
    /// </summary>
    public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// The spaceship owned by the player (if exists).
    /// </summary>
    public Spaceship? Spaceship { get; set; }
}