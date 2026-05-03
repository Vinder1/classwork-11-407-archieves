namespace AspNetApp.Entity;

/// <summary>
/// Represents a galaxy with version tracking and star collection data.
/// </summary>
public class Galaxy
{
    /// <summary>
    /// The current version number of the galaxy.
    /// </summary>
    public int Version { get; init; }

    /// <summary>
    /// The star collection providing access to all stars in the galaxy.
    /// </summary>
    public IStarCollection Stars { get; init; } = null!;
}