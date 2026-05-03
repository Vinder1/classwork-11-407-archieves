using AspNetApp.Entity;

namespace AspNetApp.IServices;

public interface IStarVisitService
{
    Task Create(Guid ownerId, Star star);

    // Returns true if there is a record with VisitedAt not older than 1 day
    Task<bool> ExistsActiveFor(Guid ownerId, Star star);
    Task<SpaceshipStarVisit?> GetActiveFor(Guid ownerId, Star star);
}