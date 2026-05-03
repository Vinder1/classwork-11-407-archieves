using AspNetApp.Dto;
using AspNetApp.Entity;

namespace AspNetApp.IServices;

public interface ISpaceshipService
{
    Task<Spaceship> CreateSpaceshipAsync(Guid ownerId, int currentGalaxyVersion);
    Task<Spaceship> GetSpaceshipAsync(Guid ownerId);
    Task<IEnumerable<Spaceship>> GetAllSpaceshipsAsync();
    Task<Spaceship> MoveSpaceshipAsync(Guid ownerId, int newRadius, double newAngle);
    Task<int> CleanupInactiveSpaceshipsAsync(int currentGalaxyVersion);
    Task<bool> HasActiveSpaceshipAsync(Guid ownerId, int currentGalaxyVersion);
}