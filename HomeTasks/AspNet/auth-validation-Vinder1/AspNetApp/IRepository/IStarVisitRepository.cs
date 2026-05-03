using AspNetApp.Entity;

namespace AspNetApp.IRepository;

/// <summary>
/// Repository interface for tracking spaceship star visit records.
/// </summary>
public interface IStarVisitRepository
{
    /// <summary>
    /// Creates a new star visit record.
    /// </summary>
    /// <param name="visit">The visit record to create.</param>
    Task Create(SpaceshipStarVisit visit);

    /// <summary>
    /// Checks if there is an active visit record within the last day.
    /// </summary>
    /// <param name="spaceship">The spaceship entity.</param>
    /// <param name="star">The star entity.</param>
    /// <returns>True if active visit exists, otherwise false.</returns>
    Task<bool> ExistsActiveFor(Spaceship spaceship, Star star);

    /// <summary>
    /// Retrieves the active visit record for a spaceship at a specific star.
    /// </summary>
    /// <param name="spaceship">The spaceship entity.</param>
    /// <param name="star">The star entity.</param>
    /// <returns>The visit record or null if not found.</returns>
    Task<SpaceshipStarVisit?> GetActiveFor(Spaceship spaceship, Star star);
}