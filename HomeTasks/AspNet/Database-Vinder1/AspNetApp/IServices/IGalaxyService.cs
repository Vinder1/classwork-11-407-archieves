using AspNetApp.Entity;

namespace AspNetApp.IServices;

public interface IGalaxyService
{
    Task<int> GetCurrentGalaxyVersionAsync();
    Task<IEnumerable<Star>> GetAllStarsAsync();

    Task<Star?> FindStarAsync(int radius, int angle);
    Task<bool> StarExistsAsync(int radius, int angle);
    Task<bool> StarExistsAsync(Star star) => StarExistsAsync(star.Radius, star.AngleMilliradians);

    Task<IEnumerable<Star>> GetStarsNearStarAsync(Star center, int radius);
    
    Task<Galaxy> RegenerateGalaxyAsync();
}