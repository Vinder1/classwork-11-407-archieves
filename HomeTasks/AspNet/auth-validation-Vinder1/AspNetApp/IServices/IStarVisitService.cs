using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// Service for tracking spaceship visits to stars.
/// </summary>
public interface IStarVisitService
{
    /// <summary>
    /// Records a visit by a spaceship owner to a specific star.
    /// </summary>
    /// <param name="ownerId">The spaceship owner's unique ID.</param>
    /// <param name="star">The visited star entity.</param>
    Task Create(Guid ownerId, Star star);

    /// <summary>
    /// Checks if there is an active visit record within the last day.
    /// </summary>
    /// <param name="ownerId">The spaceship owner's unique ID.</param>
    /// <param name="star">The star entity.</param>
    /// <returns>True if active visit exists, otherwise false.</returns>
    Task<bool> ExistsActiveFor(Guid ownerId, Star star);

    /// <summary>
    /// Retrieves the active visit record for an owner at a specific star.
    /// </summary>
    /// <param name="ownerId">The spaceship owner's unique ID.</param>
    /// <param name="star">The star entity.</param>
    /// <returns>The visit record or null if not found.</returns>
    Task<SpaceshipStarVisit?> GetActiveFor(Guid ownerId, Star star);
}