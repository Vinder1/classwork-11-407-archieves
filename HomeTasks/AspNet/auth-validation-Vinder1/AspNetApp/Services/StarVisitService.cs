namespace AspNetApp.Services;

using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;

/// <summary>
/// 
/// </summary>
/// <param name="starVisitRepository"></param>
/// <param name="galaxyService"></param>
public class StarVisitService(IStarVisitRepository starVisitRepository, IGalaxyService galaxyService) : IStarVisitService
{
    private readonly IStarVisitRepository _starVisitRepository = starVisitRepository
        ?? throw new ArgumentNullException(nameof(starVisitRepository));
    private readonly IGalaxyService _galaxyService = galaxyService
        ?? throw new ArgumentNullException(nameof(galaxyService));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visit"></param>
    /// <returns></returns>
    public Task Create(SpaceshipStarVisit visit)
    {
        return _starVisitRepository.Create(visit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="star"></param>
    /// <returns></returns>
    /// <exception cref="StarNotFoundException"></exception>
    public async Task Create(Guid ownerId, Star star)
    {
        if (!await _galaxyService.StarExistsAsync(star))
        {
            throw new StarNotFoundException();
        }
        // Давайте мирно предположим что ownerId валидный
        var spaceshipVisit = new SpaceshipStarVisit
        {
            GalaxyVersion = await _galaxyService.GetCurrentGalaxyVersionAsync(),
            Spaceship = new Spaceship { OwnerId = ownerId },
            Star = star
        };

        await Create(spaceshipVisit);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="star"></param>
    /// <returns></returns>
    /// <exception cref="StarNotFoundException"></exception>
    public async Task<bool> ExistsActiveFor(Guid ownerId, Star star)
    {
        if (!await _galaxyService.StarExistsAsync(star))
        {
            throw new StarNotFoundException();
        }
        // Давайте мирно предположим что ownerId валидный

        return await _starVisitRepository.ExistsActiveFor(new Spaceship
        {
            OwnerId = ownerId
        }, star);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="star"></param>
    /// <returns></returns>
    /// <exception cref="StarNotFoundException"></exception>
    public async Task<SpaceshipStarVisit?> GetActiveFor(Guid ownerId, Star star)
    {
        if (!await _galaxyService.StarExistsAsync(star))
        {
            throw new StarNotFoundException();
        }
        return await _starVisitRepository.GetActiveFor(new Spaceship
        {
            OwnerId = ownerId
        }, star);
    }
}