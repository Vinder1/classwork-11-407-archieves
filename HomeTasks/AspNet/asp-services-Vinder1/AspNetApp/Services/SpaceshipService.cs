using AspNetApp.DomainUtility;
using AspNetApp.Entity;
using AspNetApp.IRepository;
using AspNetApp.IServices;


namespace AspNetApp.Services;

public class SpaceshipService(
    ISpaceshipRepository spaceshipRepository,
    IPlayerService playerService) : ISpaceshipService
{
    private readonly ISpaceshipRepository _spaceshipRepository = spaceshipRepository
        ?? throw new ArgumentNullException(nameof(spaceshipRepository));
    private readonly IPlayerService _playerService = playerService
        ?? throw new ArgumentNullException(nameof(playerService));


    public async Task<Spaceship> CreateSpaceshipAsync(Guid ownerId, int currentGalaxyVersion)
    {
        if (!await _playerService.PlayerExistsAsync(ownerId))
        {
            throw new InvalidOperationException($"Cannot create spaceship: Player {ownerId} does not exist");
        }

        if (await HasActiveSpaceshipAsync(ownerId, currentGalaxyVersion))
        {
            throw new InvalidOperationException($"Cannot create spaceship: Player {ownerId} already has one");
        }

        var spaceship = await _spaceshipRepository.CreateAsync(ownerId, new Spaceship
        {
            GalaxyVersion = currentGalaxyVersion,
            OwnerId = ownerId
        });

        return spaceship;
    }

    public async Task<Spaceship> GetSpaceshipAsync(Guid ownerId)
    {
        var spaceship = await _spaceshipRepository.GetAsync(ownerId);
        return spaceship;
    }

    public async Task<IEnumerable<Spaceship>> GetAllSpaceshipsAsync()
    {
        var spaceships = await _spaceshipRepository.GetAllAsync();
        return spaceships;
    }

    public async Task<Spaceship> MoveSpaceshipAsync(Guid ownerId, int newRadius, double newAngle)
    {
        if (newRadius < 0)
        {
            throw new ArgumentException("Radius cannot be negative");
        }

        newAngle = Angle.Normalize(newAngle);

        await _spaceshipRepository.MoveAsync(ownerId, newRadius, newAngle);

        var updatedShip = await _spaceshipRepository.GetAsync(ownerId);
        return updatedShip;
    }

    public async Task<int> CleanupInactiveSpaceshipsAsync(int currentGalaxyVersion)
    {
        var beforeCleanup = await _spaceshipRepository.GetAllAsync();
        var beforeCount = beforeCleanup.Count();

        await _spaceshipRepository.DeleteInactiveAsync(currentGalaxyVersion);

        var afterCleanup = await _spaceshipRepository.GetAllAsync();
        var afterCount = afterCleanup.Count();

        var removedCount = beforeCount - afterCount;

        return removedCount;
    }

    public async Task<bool> HasActiveSpaceshipAsync(Guid ownerId, int currentGalaxyVersion)
    {
        try
        {
            var ship = await _spaceshipRepository.GetAsync(ownerId);
            return ship.GalaxyVersion >= currentGalaxyVersion;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}