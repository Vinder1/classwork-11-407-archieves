namespace AspNetApp.IServices;

/// <summary>
/// Service for generating and managing JWT tokens.
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified player.
    /// </summary>
    /// <param name="player">The player entity.</param>
    /// <returns>The generated JWT token string.</returns>
    string GenerateToken(Entity.UserDto player);
}