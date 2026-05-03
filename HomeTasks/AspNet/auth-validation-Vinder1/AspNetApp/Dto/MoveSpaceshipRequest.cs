namespace AspNetApp.Dto;

/// <summary>
/// Represents a request to move a spaceship to new coordinates.
/// </summary>
public class MoveSpaceshipRequest
{
    /// <summary>
    /// The unique identifier of the spaceship owner.
    /// </summary>
    public Guid OwnerId { get; set; }

    /// <summary>
    /// The target radius coordinate.
    /// </summary>
    public int NewRadius { get; set; }

    /// <summary>
    /// The target angle coordinate in milliradians.
    /// </summary>
    public int NewAngle { get; set; }
}