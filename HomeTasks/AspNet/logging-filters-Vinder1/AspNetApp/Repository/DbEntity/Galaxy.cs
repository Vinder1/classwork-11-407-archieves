namespace AspNetApp.Repository.DbEntity;

/// <summary>
/// 
/// </summary>
public class Galaxy
{
    /// <summary>
    /// 
    /// </summary>
    public int Version { get; init; } // Primary Key
    /// <summary>
    /// 
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    /// <summary>
    /// 
    /// </summary>
    public ICollection<Star> Stars { get; init; } = [];
}