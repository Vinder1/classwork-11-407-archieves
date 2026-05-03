using AspNetApp.Entity;
using AspNetApp.IRepository.Template;

namespace AspNetApp.IRepository;

public interface ISpaceshipRepository : IRepository<Spaceship, Guid>
{
    Task MoveAsync(Guid owner, int newRadius, int newAngleMilliradians);
    Task<IEnumerable<Spaceship>> GetAllAsync();
    Task<IEnumerable<Spaceship>> GetAllOnStarAsync(int radius, int angleMilliradians);

    // Remove all spaceships where ship.GalaxyVersion < galaxyVersion 
    Task DeleteInactiveAsync(int galaxyVersion);
}