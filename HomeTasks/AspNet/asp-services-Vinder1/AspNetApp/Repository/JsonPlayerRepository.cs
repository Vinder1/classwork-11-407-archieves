using System.Text.Json;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetApp.Repository;

public class JsonPlayerRepository : IPlayerRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private const string AllPlayersCacheKey = "AllPlayers";

    public JsonPlayerRepository(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        _cache = new MemoryCache(new MemoryCacheOptions());
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            SlidingExpiration = TimeSpan.FromMinutes(1),
            Priority = CacheItemPriority.Normal
        };
        InitializeFile();
    }

    private void InitializeFile()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Player> CreateAsync(Guid id, Player entity)
    {
        await _fileLock.WaitAsync();
        try
        {
            var players = await ReadPlayersAsync();

            players.Add(entity);
            await WritePlayersAsync(players);

            return entity;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<Player> GetAsync(Guid id)
    {
        if (_cache.TryGetValue(AllPlayersCacheKey, out List<Player>? cachedPlayers) && cachedPlayers != null)
        {
            var player = cachedPlayers.FirstOrDefault(p => p.Id == id);
            if (player != null)
                return player;
        }

        await _fileLock.WaitAsync();
        try
        {
            var players = await ReadPlayersAsync();

            var player = players.FirstOrDefault(p => p.Id == id);

            return player ?? throw new KeyNotFoundException($"Player with id {id} not found");
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task UpdateAsync(Guid id, Player entity)
    {
        await _fileLock.WaitAsync();
        try
        {
            var players = await ReadPlayersAsync();

            var player = players.FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException($"Player with id {id} not found");

            players.Remove(player);
            players.Add(entity);

            await WritePlayersAsync(players);
        }
        finally
        {
            _fileLock.Release();
        }
    }


    public async Task DeleteAsync(Guid id)
    {
        await _fileLock.WaitAsync();
        try
        {
            var players = await ReadPlayersAsync();
            var player = players.FirstOrDefault(p => p.Id == id);

            if (player == null)
                return;

            players.Remove(player);
            await WritePlayersAsync(players);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task<List<Player>> ReadPlayersAsync()
    {
        await using FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        return await JsonSerializer.DeserializeAsync<List<Player>>(fs, JsonSerializationParams.Options) ?? [];
    }

    private async Task WritePlayersAsync(List<Player> players)
    {
        await using FileStream fs = new(_filePath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(fs, players, JsonSerializationParams.Options);
        _cache.Set(AllPlayersCacheKey, players, _cacheOptions);
    }
}