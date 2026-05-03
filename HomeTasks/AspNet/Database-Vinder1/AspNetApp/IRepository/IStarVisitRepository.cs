using AspNetApp.Entity;

namespace AspNetApp.IRepository;

public interface IStarVisitRepository
{
    Task Create(SpaceshipStarVisit visit);

    // Returns true if there is a record with VisitedAt not older than 1 day
    Task<bool> ExistsActiveFor(Spaceship spaceship, Star star);
    Task<SpaceshipStarVisit?> GetActiveFor(Spaceship spaceship, Star star);
}