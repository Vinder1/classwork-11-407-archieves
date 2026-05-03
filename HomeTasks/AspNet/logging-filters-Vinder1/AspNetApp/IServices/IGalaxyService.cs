using AspNetApp.Entity;

namespace AspNetApp.IServices;

/// <summary>
/// Service for managing galaxy-related operations.
/// </summary>
public interface IGalaxyService
{
    /// <summary>
    /// Gets the current version number of the galaxy.
    /// </summary>
    /// <returns>The galaxy version as an integer.</returns>
    Task<int> GetCurrentGalaxyVersionAsync();

    /// <summary>
    /// Retrieves all stars in the galaxy.
    /// </summary>
    /// <returns>A collection of all star entities.</returns>
    Task<IEnumerable<Star>> GetAllStarsAsync();

    /// <summary>
    /// Finds a star by its position coordinates.
    /// </summary>
    /// <param name="radius">The radius value.</param>
    /// <param name="angle">The angle in milliradians.</param>
    /// <returns>The found star or null if not exists.</returns>
    Task<Star?> FindStarAsync(int radius, int angle);

    /// <summary>
    /// Checks if a star exists at given coordinates.
    /// </summary>
    /// <param name="radius">The radius value.</param>
    /// <param name="angle">The angle in milliradians.</param>
    /// <returns>True if star exists, otherwise false.</returns>
    Task<bool> StarExistsAsync(int radius, int angle);

    /// <summary>
    /// Checks if a star entity exists in the system.
    /// </summary>
    /// <param name="star">The star to check.</param>
    /// <returns>True if star exists, otherwise false.</returns>
    Task<bool> StarExistsAsync(Star star) => StarExistsAsync(star.Radius, star.AngleMilliradians);

    /// <summary>
    /// Gets stars located near a specified center star.
    /// </summary>
    /// <param name="center">The center star for proximity calculation.</param>
    /// <param name="radius">Search radius around center star.</param>
    /// <returns>Collection of nearby stars.</returns>
    Task<IEnumerable<Star>> GetStarsNearStarAsync(Star center, int radius);

    /// <summary>
    /// Regenerates the entire galaxy structure.
    /// </summary>
    /// <returns>The newly generated galaxy instance.</returns>
    Task<Galaxy> RegenerateGalaxyAsync();
}