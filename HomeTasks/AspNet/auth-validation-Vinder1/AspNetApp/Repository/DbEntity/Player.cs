namespace AspNetApp.Repository.DbEntity;

/// <summary>
/// 
/// </summary>
public class Player
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Login { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string PasswordHash { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Role { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;
    /// <summary>
    /// 
    /// </summary>
    public Spaceship? Spaceship { get; set; }
}