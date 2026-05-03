using AspNetApp.Entity;

namespace AspNetApp.IServices;

public interface IPlayerService
{
    Task<Player> CreatePlayerAsync(string name);
    Task<Player> GetPlayerAsync(Guid id);
    Task<Player> UpdatePlayerNameAsync(Guid id, string newName);
    Task DeletePlayerAsync(Guid id);
    Task<bool> PlayerExistsAsync(Guid id);
}