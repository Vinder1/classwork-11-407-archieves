namespace AspNetApp.Entity;

/// <summary>
/// Represents a spaceship entity with owner and location information.
/// </summary>
public class Spaceship
{
    /// <summary>
    /// Unique identifier of the spaceship owner.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Galaxy version when the spaceship was created.
    /// </summary>
    public int GalaxyVersion { get; init; }

    /// <summary>
    /// The current radius coordinate where the spaceship is located.
    /// </summary>
    public int LocatedRadius { get; set; }

    /// <summary>
    /// The current angle coordinate in milliradians.
    /// </summary>
    public int LocatedAngleMilliradians { get; set; }

    /// <summary>
    /// The player who owns this spaceship.
    /// </summary>
    public Player Owner { get; init; } = null!;

    /// <summary>
    /// The collection of star visits made by this spaceship.
    /// </summary>
    public ICollection<SpaceshipStarVisit> VisitedStars { get; init; } = [];
}