namespace AspNetApp.Dto;

/// <summary>
/// Represents a request to create a new spaceship for an existing player.
/// </summary>
public class CreateSpaceshipRequest
{
    /// <summary>
    /// The unique identifier of the owner who will receive the spaceship.
    /// </summary>
    public Guid OwnerId { get; init; }
}