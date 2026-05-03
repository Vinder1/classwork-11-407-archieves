namespace AspNetApp.Entity;

/// <summary>
/// Represents a star entity with coordinate information and visit tracking.
/// </summary>
public readonly struct Star
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Star() {}

    /// <summary>
    /// The radius coordinate of the star.
    /// </summary>
    public int Radius { get; init; }

    /// <summary>
    /// The angle coordinate in milliradians (0-6283).
    /// </summary>
    public int AngleMilliradians { get; init; }

    /// <summary>
    /// The collection of spaceship visits to this star.
    /// </summary>
    public ICollection<SpaceshipStarVisit> VisitedByShips { get; init; } = [];
}