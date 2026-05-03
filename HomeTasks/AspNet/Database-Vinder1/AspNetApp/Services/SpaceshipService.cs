using AspNetApp.DomainUtility;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;


namespace AspNetApp.Services;

public class SpaceshipService(
    ISpaceshipRepository spaceshipRepository,
    IPlayerService playerService,
    IGalaxyService galaxyService) : ISpaceshipService
{
    private readonly ISpaceshipRepository _spaceshipRepository = spaceshipRepository
        ?? throw new ArgumentNullException(nameof(spaceshipRepository));
    private readonly IPlayerService _playerService = playerService
        ?? throw new ArgumentNullException(nameof(playerService));
    private readonly IGalaxyService _galaxyService = galaxyService
        ?? throw new ArgumentNullException(nameof(galaxyService));

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

    public Task<Spaceship> GetSpaceshipAsync(Guid ownerId)
    {
        return _spaceshipRepository.GetAsync(ownerId);
    }

    public Task<IEnumerable<Spaceship>> GetAllSpaceshipsAsync()
    {
        return _spaceshipRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Spaceship>> GetAllOnStarAsync(Star star)
    {
        if (!await _galaxyService.StarExistsAsync(star))
        {
            throw new ArgumentException("Star not found");
        }

        var spaceships = await _spaceshipRepository.GetAllOnStarAsync(star.Radius, star.AngleMilliradians);
        return spaceships;
    }

    public async Task<Spaceship> MoveSpaceshipAsync(Guid ownerId, Star newStar)
    {
        var newRadius = newStar.Radius;
        var newAngleMilliradians = newStar.AngleMilliradians;

        if (newRadius < 0)
        {
            throw new ArgumentException("Radius cannot be negative");
        }

        newAngleMilliradians = Angle.NormalizeMilliradians(newAngleMilliradians);

        if (!await _galaxyService.StarExistsAsync(newRadius, newAngleMilliradians))
        {
            throw new StarNotFoundException();
        }

        await _spaceshipRepository.MoveAsync(ownerId, newRadius, newAngleMilliradians);

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
            if (ship == null)
                return false;
            return ship.GalaxyVersion >= currentGalaxyVersion;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
}