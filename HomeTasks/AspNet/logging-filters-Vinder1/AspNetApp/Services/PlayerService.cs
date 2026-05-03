using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;

namespace AspNetApp.Services;

/// <summary>
/// 
/// </summary>
/// <param name="playerRepository"></param>
/// <param name="logger"></param>
public class PlayerService(
    IPlayerRepository playerRepository,
    ILogger<PlayerService> logger) : IPlayerService
{
    private readonly IPlayerRepository _playerRepository = playerRepository
        ?? throw new ArgumentNullException(nameof(playerRepository));
    private readonly ILogger<PlayerService> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

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
        ArgumentNullException.ThrowIfNull(player);

        await _playerRepository.UpdateNameAsync(id, newName);
        var updatedPlayer = await _playerRepository.GetAsync(id);

        _logger.LogInformation("User (id={UserId}): Changed name to {NewName}", updatedPlayer.Id, updatedPlayer.Name);

        return updatedPlayer;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task DeletePlayerAsync(Guid id)
    {
        _logger.LogInformation("User (id={UserId}): Deleted his account", id);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    public async Task<bool> PlayerLoginExistsAsync(string login)
    {
        var player = await _playerRepository.GetByLoginAsync(login);
        return player != null;
    }
}