using AspNetApp.Entity;
using AspNetApp.IRepository.Template;

namespace AspNetApp.IRepository;

/// <summary>
/// Repository for spaceship-related data operations.
/// </summary>
public interface ISpaceshipRepository : IRepository<Spaceship, Guid>
{
    /// <summary>
    /// Updates the location of a spaceship for a specific owner.
    /// </summary>
    /// <param name="owner">The owner's unique identifier.</param>
    /// <param name="newRadius">The new radius coordinate.</param>
    /// <param name="newAngleMilliradians">The new angle in milliradians.</param>
    Task MoveAsync(Guid owner, int newRadius, int newAngleMilliradians);

    /// <summary>
    /// Retrieves all spaceships from the repository.
    /// </summary>
    /// <returns>A collection of all spaceship entities.</returns>
    Task<IEnumerable<Spaceship>> GetAllAsync();

    /// <summary>
    /// Retrieves all spaceships located on a star at the specified coordinates.
    /// </summary>
    /// <param name="radius">The radius coordinate.</param>
    /// <param name="angleMilliradians">The angle in milliradians.</param>
    /// <returns>Collection of spaceships at the given star location.</returns>
    Task<IEnumerable<Spaceship>> GetAllOnStarAsync(int radius, int angleMilliradians);

    /// <summary>
    /// Removes inactive spaceships that were created on older galaxies.
    /// </summary>
    /// <param name="galaxyVersion">The current galaxy version threshold.</param>
    Task DeleteInactiveAsync(int galaxyVersion);
}