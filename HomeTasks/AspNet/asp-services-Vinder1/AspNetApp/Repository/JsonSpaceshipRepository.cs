using System.Text.Json;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetApp.Repository;

public class JsonSpaceshipRepository : ISpaceshipRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private const string AllSpaceshipsCacheKey = "AllSpaceships";

    public JsonSpaceshipRepository(string filePath)
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

    public async Task<Spaceship> CreateAsync(Guid id, Spaceship entity)
    {
        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();

            var existingShip = spaceships.Find(s => s.OwnerId == entity.OwnerId);
            if (existingShip != null)
            {
                if (existingShip.GalaxyVersion >= entity.GalaxyVersion)
                {
                    throw new InvalidOperationException($"Spaceship for owner {entity.OwnerId} already exists");
                }
                else
                {
                    await DeleteAsync(existingShip.OwnerId);
                }
            }

            spaceships.Add(entity);
            await WriteSpaceshipsAsync(spaceships);

            return entity;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<Spaceship> GetAsync(Guid owner)
    {
        if (_cache.TryGetValue(AllSpaceshipsCacheKey, out List<Spaceship>? cachedShips) && cachedShips != null)
        {
            var ship = cachedShips.Find(s => s.OwnerId == owner);
            if (ship != null)
                return ship;
        }

        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();

            var ship = spaceships.FirstOrDefault(s => s.OwnerId == owner);
            return ship ?? throw new KeyNotFoundException($"Spaceship for owner {owner} not found");
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task UpdateAsync(Guid id, Spaceship entity)
    {
        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();
            spaceships.RemoveAll(s => s.OwnerId == entity.OwnerId);
            spaceships.Add(entity);

            await WriteSpaceshipsAsync(spaceships);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task MoveAsync(Guid owner, int newRadius, double newAngle)
    {
        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();
            var shipIndex = spaceships.FindIndex(s => s.OwnerId == owner);

            if (shipIndex == -1)
                throw new KeyNotFoundException($"Spaceship for owner {owner} not found");

            spaceships[shipIndex].LocatedRadius = newRadius;
            spaceships[shipIndex].LocatedAngle = newAngle;

            await WriteSpaceshipsAsync(spaceships);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<IEnumerable<Spaceship>> GetAllAsync()
    {
        if (_cache.TryGetValue(AllSpaceshipsCacheKey, out List<Spaceship>? cachedShips) && cachedShips != null)
        {
            return cachedShips.AsReadOnly();
        }

        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();

            return spaceships.AsReadOnly();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task DeleteInactiveAsync(int galaxyVersion)
    {
        await _fileLock.WaitAsync();
        try
        {
            var spaceships = await ReadSpaceshipsAsync();

            var removedCount = spaceships.RemoveAll(s => s.GalaxyVersion < galaxyVersion);

            if (removedCount > 0)
            {
                await WriteSpaceshipsAsync(spaceships);
            }
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
            var spaceships = await ReadSpaceshipsAsync();

            var removedCount = spaceships.RemoveAll(s => s.OwnerId == id);

            if (removedCount > 0)
            {
                await WriteSpaceshipsAsync(spaceships);
            }
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private async Task<List<Spaceship>> ReadSpaceshipsAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Spaceship>>(json, JsonSerializationParams.Options) ?? [];
    }

    private async Task WriteSpaceshipsAsync(List<Spaceship> spaceships)
    {
        var json = JsonSerializer.Serialize(spaceships, JsonSerializationParams.Options);
        await File.WriteAllTextAsync(_filePath, json);
        _cache.Set(AllSpaceshipsCacheKey, spaceships, _cacheOptions);
    }
}