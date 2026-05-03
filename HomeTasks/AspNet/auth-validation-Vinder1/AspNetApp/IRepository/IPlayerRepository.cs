using AspNetApp.Entity;
using AspNetApp.IRepository.Template;

namespace AspNetApp.IRepository;

/// <summary>
/// Repository for Player
/// </summary>
public interface IPlayerRepository : IRepository<Player, Guid>
{
    /// <summary>
    /// Retrieve the player by his login
    /// </summary>
    /// <param name="login"></param>
    /// <returns>The player, or null, if missing</returns>
    Task<Player?> GetByLoginAsync(string login);

    /// <summary>
    /// Update name of player
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    Task UpdateNameAsync(Guid id, string newName);
}