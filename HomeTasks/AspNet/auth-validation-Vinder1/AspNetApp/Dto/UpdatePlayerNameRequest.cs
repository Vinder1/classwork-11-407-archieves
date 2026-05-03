namespace AspNetApp.Dto;

/// <summary>
/// Represents a request to change user`s name
/// </summary>
public class UpdatePlayerNameRequest
{
    /// <summary>
    /// The identifier of user
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// The new name of user
    /// </summary>
    public string Name { get; init; } = null!;
}