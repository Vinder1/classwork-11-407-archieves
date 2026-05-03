using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;

namespace AspNetApp.Services;

/// <summary>
/// 
/// </summary>
/// <param name="playerRepository"></param>
public class PlayerService(IPlayerRepository playerRepository) : IPlayerService
{
    private readonly IPlayerRepository _playerRepository = playerRepository
        ?? throw new ArgumentNullException(nameof(playerRepository));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<Player> GetPlayerAsync(Guid id)
    {
        return _playerRepository.GetAsync(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public async Task<Player> UpdatePlayerNameAsync(Guid id, string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        var player = await _playerRepository.GetAsync(id);

        await _playerRepository.UpdateNameAsync(id, newName);
        var updatedPlayer = await _playerRepository.GetAsync(id);

        return updatedPlayer;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeletePlayerAsync(Guid id)
    {
        return _playerRepository.DeleteAsync(id);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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