namespace AspNetApp.Entity;

/// <summary>
/// Represents a spaceship visit record to a star.
/// </summary>
public readonly struct SpaceshipStarVisit()
{
    /// <summary>
    /// The timestamp when the star was visited (UTC).
    /// </summary>
    public DateTime VisitedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// The galaxy version at the time of visit.
    /// </summary>
    public int GalaxyVersion { get; init; }

    /// <summary>
    /// The spaceship that made the visit.
    /// </summary>
    public Spaceship Spaceship { get; init; } = null!;

    /// <summary>
    /// The star that was visited.
    /// </summary>
    public Star Star { get; init; } = default;
}