namespace AspNetApp.Entity;

/// <summary>
/// Represents a collection of stars with efficient lookup capabilities.
/// </summary>
public interface IStarCollection
{
    /// <summary>
    /// The 2D array of stars organized by radius.
    /// </summary>
    public Star[][] Stars { get; }

    /// <summary>
    /// Finds a star at the specified coordinates.
    /// </summary>
    /// <param name="radius">The radius value.</param>
    /// <param name="angleMilliradians">The angle in milliradians.</param>
    /// <returns>The found star or null if not exists.</returns>
    public Star? Find(int radius, int angleMilliradians);

    /// <summary>
    /// Retrieves all stars from the collection.
    /// </summary>
    /// <returns>A collection of all star entities.</returns>
    public IEnumerable<Star> GetAllStars();

    /// <summary>
    /// Finds stars located near a specified point within the given radius range.
    /// </summary>
    /// <param name="sourceRadius">The source radius.</param>
    /// <param name="sourceAngleMilliradians">The source angle in milliradians.</param>
    /// <param name="radius">The search radius around the source point.</param>
    /// <returns>Collection of nearby stars.</returns>
    public IEnumerable<Star> FindNearbyStars(int sourceRadius, int sourceAngleMilliradians, int radius);
}