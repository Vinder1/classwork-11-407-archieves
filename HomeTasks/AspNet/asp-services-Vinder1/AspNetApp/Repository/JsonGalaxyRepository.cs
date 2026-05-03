using System.Text.Json;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetApp.Repository;

public class JsonGalaxyRepository : IGalaxyRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    private const string GalaxyCacheKey = "CurrentGalaxy";

    public JsonGalaxyRepository(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

        _cache = new MemoryCache(new MemoryCacheOptions());
        _cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            SlidingExpiration = TimeSpan.FromMinutes(5),
            Priority = CacheItemPriority.High
        };

        InitializeDirectory();
    }

    private void InitializeDirectory()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task<Galaxy?> GetAsync()
    {
        if (_cache.TryGetValue(GalaxyCacheKey, out Galaxy? cachedGalaxy) && cachedGalaxy != null)
        {
            return cachedGalaxy;
        }

        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            await using FileStream fs = new(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            var jsonGalaxy = await JsonSerializer.DeserializeAsync<JsonGalaxy>(fs, JsonSerializationParams.Options)
                ?? throw new InvalidOperationException("Failed to deserialize galaxy data");

            var galaxy = FromJsonGalaxy(jsonGalaxy);

            _cache.Set(GalaxyCacheKey, galaxy, _cacheOptions);

            return galaxy;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task OverrideAsync(Galaxy entity)
    {
        await _fileLock.WaitAsync();
        try
        {
            var newGalaxy = new JsonGalaxy(entity.Version, entity.Stars.Stars);

            await using FileStream fs = new(_filePath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(fs, newGalaxy, JsonSerializationParams.Options);

            _cache.Set(GalaxyCacheKey, newGalaxy, _cacheOptions);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    private static Galaxy FromJsonGalaxy(JsonGalaxy jsonGalaxy)
    {
        return new Galaxy
        {
            Version = jsonGalaxy.Version,
            Stars = new StarCollection(jsonGalaxy.Stars)
        };
    }
}