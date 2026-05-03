using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// Service for managing spaceship operations and data.
/// </summary>
public interface ISpaceshipService
{
    /// <summary>
    /// Creates a new spaceship for the specified owner.
    /// </summary>
    /// <param name="ownerId">The owner's unique ID.</param>
    /// <param name="currentGalaxyVersion">The current galaxy version.</param>
    /// <returns>The newly created spaceship entity.</returns>
    Task<Spaceship> CreateSpaceshipAsync(Guid ownerId, int currentGalaxyVersion);

    /// <summary>
    /// Retrieves a spaceship by its owner's unique identifier.
    /// </summary>
    /// <param name="ownerId">The owner's unique ID.</param>
    /// <returns>The spaceship entity.</returns>
    Task<Spaceship> GetSpaceshipAsync(Guid ownerId);

    /// <summary>
    /// Retrieves all spaceships in the system.
    /// </summary>
    /// <returns>A collection of all spaceship entities.</returns>
    Task<IEnumerable<Spaceship>> GetAllSpaceshipsAsync();

    /// <summary>
    /// Gets all spaceships located on a specific star.
    /// </summary>
    /// <param name="star">The star entity.</param>
    /// <returns>Collection of spaceships on the star.</returns>
    Task<IEnumerable<Spaceship>> GetAllOnStarAsync(Star star);

    /// <summary>
    /// Moves a spaceship to a new star.
    /// </summary>
    /// <param name="ownerId">The owner's unique ID.</param>
    /// <param name="newStar">The destination star.</param>
    /// <returns>The updated spaceship entity.</returns>
    Task<Spaceship> MoveSpaceshipAsync(Guid ownerId, Star newStar);

    /// <summary>
    /// Cleans up inactive spaceships based on galaxy version.
    /// </summary>
    /// <param name="currentGalaxyVersion">The current galaxy version.</param>
    /// <returns>The number of removed ships.</returns>
    Task<int> CleanupInactiveSpaceshipsAsync(int currentGalaxyVersion);

    /// <summary>
    /// Checks if an owner has an active spaceship.
    /// </summary>
    /// <param name="ownerId">The owner's unique ID.</param>
    /// <param name="currentGalaxyVersion">The current galaxy version.</param>
    /// <returns>True if active spaceship exists, otherwise false.</returns>
    Task<bool> HasActiveSpaceshipAsync(Guid ownerId, int currentGalaxyVersion);
}