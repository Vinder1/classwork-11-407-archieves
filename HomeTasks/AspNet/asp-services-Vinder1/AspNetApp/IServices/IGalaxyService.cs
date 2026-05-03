using AspNetApp.Entity;

namespace AspNetApp.IServices;

public interface IGalaxyService
{
    Task<int> GetCurrentGalaxyVersionAsync();
    Task<IEnumerable<Star>> GetAllStarsAsync();
    Task<Star?> FindStarAsync(int radius, double angle);
    Task<IEnumerable<Star>> GetStarsNearStarAsync(Star center, int radius);
    
    Task<Galaxy> RegenerateGalaxyAsync();
}