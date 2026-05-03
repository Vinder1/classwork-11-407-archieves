namespace AspNetApp.Dto;

/// <summary>
/// Represents a request to visit a star with spaceship location update.
/// </summary>
public class StarVisitRequest
{
    /// <summary>
    /// The unique identifier of the spaceship owner.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    ///The target radius coordinate.
    /// </summary>
    public int Radius { get; set; }

    /// <summary>
    /// The target angle coordinate in milliradians.
    /// </summary>
    public int AngleMilliradians { get; set; }
}