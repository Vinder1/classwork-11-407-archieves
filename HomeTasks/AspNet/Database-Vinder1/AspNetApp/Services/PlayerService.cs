using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;

namespace AspNetApp.Services;

public class PlayerService(IPlayerRepository playerRepository) : IPlayerService
{
    private readonly IPlayerRepository _playerRepository = playerRepository
        ?? throw new ArgumentNullException(nameof(playerRepository));

    public Task<Player> CreatePlayerAsync(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (name.Length < 3 || name.Length > 50)
        {
            throw new ArgumentException("Player name must be between 3 and 50 characters");
        }
        
        var guid = Guid.NewGuid();

        return _playerRepository.CreateAsync(guid, new Player
        {
            Id=guid,
            Name=name
        });
    }

    public Task<Player> GetPlayerAsync(Guid id)
    {
        return _playerRepository.GetAsync(id);
    }

    public async Task<Player> UpdatePlayerNameAsync(Guid id, string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);

        await _playerRepository.UpdateAsync(id, new Player
        {
            Id=id,
            Name=newName
        });
        var updatedPlayer = await _playerRepository.GetAsync(id);

        return updatedPlayer;
    }

    public Task DeletePlayerAsync(Guid id)
    {
        return _playerRepository.DeleteAsync(id);
    }

    public async Task<bool> PlayerExistsAsync(Guid id)
    {
        try
        {
            await _playerRepository.GetAsync(id);
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}