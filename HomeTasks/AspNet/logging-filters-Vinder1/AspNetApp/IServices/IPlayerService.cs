using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// Service for managing player data and operations.
/// </summary>
public interface IPlayerService
{
    /// <summary>
    /// Retrieves a player by their unique identifier.
    /// </summary>
    /// <param name="id">The player's unique ID.</param>
    /// <returns>The player entity.</returns>
    Task<Player> GetPlayerAsync(Guid id);

    /// <summary>
    /// Updates the name of an existing player.
    /// </summary>
    /// <param name="id">The player's unique ID.</param>
    /// <param name="newName">The new player name.</param>
    /// <returns>The updated player entity.</returns>
    Task<Player> UpdatePlayerNameAsync(Guid id, string newName);

    /// <summary>
    /// Deletes a player by their unique identifier.
    /// </summary>
    /// <param name="id">The player's unique ID.</param>
    Task DeletePlayerAsync(Guid id);

    /// <summary>
    /// Checks if a player exists with the given ID.
    /// </summary>
    /// <param name="id">The player's unique ID.</param>
    /// <returns>True if player exists, otherwise false.</returns>
    Task<bool> PlayerExistsAsync(Guid id);
    
    /// <summary>
    /// Checks if a player exists with the given login.
    /// </summary>
    /// <param name="login">The player's unique login.</param>
    /// <returns>True if player exists, otherwise false.</returns>
    Task<bool> PlayerLoginExistsAsync(string login);
}