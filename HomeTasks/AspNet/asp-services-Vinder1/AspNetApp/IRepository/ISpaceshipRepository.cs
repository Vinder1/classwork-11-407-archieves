using AspNetApp.Entity;
using AspNetApp.IRepository.Template;

namespace AspNetApp.IRepository;

public interface ISpaceshipRepository : IRepository<Spaceship, Guid>
{
    public Task MoveAsync(Guid owner, int newRadius, double newAngle);
    public Task<IEnumerable<Spaceship>> GetAllAsync();

    // Remove all spaceships where ship.GalaxyVersion < galaxyVersion 
    public Task DeleteInactiveAsync(int galaxyVersion);
}