using AspNetApp.DomainUtility;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;


namespace AspNetApp.Services;

/// <summary>
/// 
/// </summary>
/// <param name="galaxyRepository"></param>
public class GalaxyService(IGalaxyRepository galaxyRepository) : IGalaxyService
{
    private readonly IGalaxyRepository _galaxyRepository = galaxyRepository
        ?? throw new ArgumentNullException(nameof(galaxyRepository));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GalaxyMissingException"></exception>
    public async Task<int> GetCurrentGalaxyVersionAsync()
    {
        return (await _galaxyRepository.GetAsync() ?? throw new GalaxyMissingException()).Version;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Galaxy> RegenerateGalaxyAsync()
    {
        var currentGalaxy = await _galaxyRepository.GetAsync();
        var newVersion = (currentGalaxy?.Version + 1) ?? 1;

        var stars = GalaxyGenerator.GenerateStars();
        var newGalaxy = GalaxyGenerator.CreateGalaxy(newVersion, stars);

        await _galaxyRepository.OverrideAsync(newGalaxy);

        return newGalaxy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GalaxyMissingException"></exception>
    /// <exception cref="NullReferenceException"></exception>
    public async Task<IEnumerable<Star>> GetAllStarsAsync()
    {
        var galaxy = await _galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy?.Stars.GetAllStars() ?? throw new NullReferenceException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angleMilliradians"></param>
    /// <returns></returns>
    /// <exception cref="GalaxyMissingException"></exception>
    public async Task<Star?> FindStarAsync(int radius, int angleMilliradians)
    {
        var galaxy = await galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy.Stars.Find(radius, angleMilliradians);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public async Task<bool> StarExistsAsync(int radius, int angle)
    {
        return (await FindStarAsync(radius, angle)) != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    /// <exception cref="GalaxyMissingException"></exception>
    public async Task<IEnumerable<Star>> GetStarsNearStarAsync(Star center, int radius)
    {
        var galaxy = await galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy.Stars.FindNearbyStars(center.Radius, center.AngleMilliradians, radius);
    }
}