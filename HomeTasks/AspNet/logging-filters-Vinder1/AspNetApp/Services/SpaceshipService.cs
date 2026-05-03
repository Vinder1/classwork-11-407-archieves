using AspNetApp.DomainUtility;
using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;


namespace AspNetApp.Services;

/// <summary>
/// 
/// </summary>
/// <param name="spaceshipRepository"></param>
/// <param name="playerService"></param>
/// <param name="galaxyService"></param>
/// <param name="logger"></param>
public class SpaceshipService(
    ISpaceshipRepository spaceshipRepository,
    IPlayerService playerService,
    IGalaxyService galaxyService,
    ILogger<SpaceshipService> logger) : ISpaceshipService
{
    private readonly ISpaceshipRepository _spaceshipRepository = spaceshipRepository
        ?? throw new ArgumentNullException(nameof(spaceshipRepository));
    private readonly IPlayerService _playerService = playerService
        ?? throw new ArgumentNullException(nameof(playerService));
    private readonly IGalaxyService _galaxyService = galaxyService
        ?? throw new ArgumentNullException(nameof(galaxyService));
    private readonly ILogger<SpaceshipService> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="currentGalaxyVersion"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
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

        _logger.LogInformation("Spaceship (owner={UserId}): Created for Galaxy {GalaxyVersion}", ownerId, currentGalaxyVersion);

        return spaceship;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    public Task<Spaceship> GetSpaceshipAsync(Guid ownerId)
    {
        return _spaceshipRepository.GetAsync(ownerId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<Spaceship>> GetAllSpaceshipsAsync()
    {
        return _spaceshipRepository.GetAllAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="star"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<IEnumerable<Spaceship>> GetAllOnStarAsync(Star star)
    {
        if (!await _galaxyService.StarExistsAsync(star))
        {
            throw new ArgumentException("Star not found");
        }

        var spaceships = await _spaceshipRepository.GetAllOnStarAsync(star.Radius, star.AngleMilliradians);
        return spaceships;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="newStar"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="StarNotFoundException"></exception>
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

        _logger.LogInformation("Spaceship (owner={UserId}): Moved to R={Radius}, A={Angle}",
            ownerId, updatedShip.LocatedRadius, updatedShip.LocatedAngleMilliradians);
        
        return updatedShip;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentGalaxyVersion"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="currentGalaxyVersion"></param>
    /// <returns></returns>
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