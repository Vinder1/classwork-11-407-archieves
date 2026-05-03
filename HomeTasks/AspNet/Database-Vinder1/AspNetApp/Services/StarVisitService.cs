namespace AspNetApp.Services;

using AspNetApp.Entity;
using AspNetApp.Exceptions;
using AspNetApp.IRepository;
using AspNetApp.IServices;

public class StarVisitService(IStarVisitRepository starVisitRepository, IGalaxyService galaxyService) : IStarVisitService
{
    private readonly IStarVisitRepository _starVisitRepository = starVisitRepository
        ?? throw new ArgumentNullException(nameof(starVisitRepository));
    private readonly IGalaxyService _galaxyService = galaxyService
        ?? throw new ArgumentNullException(nameof(galaxyService));

    public Task Create(SpaceshipStarVisit visit)
    {
        return _starVisitRepository.Create(visit);
    }

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