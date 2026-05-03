using AspNetApp.DomainUtility;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;


namespace AspNetApp.Services;

public class GalaxyService(IGalaxyRepository galaxyRepository) : IGalaxyService
{
    private readonly IGalaxyRepository _galaxyRepository = galaxyRepository
        ?? throw new ArgumentNullException(nameof(galaxyRepository));

    public async Task<int> GetCurrentGalaxyVersionAsync()
    {
        return (await _galaxyRepository.GetAsync() ?? throw new GalaxyMissingException()).Version;
    }

    public async Task<Galaxy> RegenerateGalaxyAsync()
    {
        var currentGalaxy = await _galaxyRepository.GetAsync();
        var newVersion = (currentGalaxy?.Version + 1) ?? 1;

        var stars = GalaxyGenerator.GenerateStars();
        var newGalaxy = GalaxyGenerator.CreateGalaxy(newVersion, stars);

        await _galaxyRepository.OverrideAsync(newGalaxy);

        return newGalaxy;
    }

    public async Task<IEnumerable<Star>> GetAllStarsAsync()
    {
        var galaxy = await _galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy?.Stars.GetAllStars() ?? throw new NullReferenceException();
    }

    public async Task<Star?> FindStarAsync(int radius, double angle)
    {
        var galaxy = await galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy.Stars.Find(radius, angle);
    }

    public async Task<IEnumerable<Star>> GetStarsNearStarAsync(Star center, int radius)
    {
        var galaxy = await galaxyRepository.GetAsync() ?? throw new GalaxyMissingException();
        return galaxy.Stars.FindNearbyStars(center.Radius, center.Angle, radius);
    }
}