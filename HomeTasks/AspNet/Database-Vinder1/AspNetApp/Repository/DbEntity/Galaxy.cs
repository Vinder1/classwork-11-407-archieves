namespace AspNetApp.Repository.DbEntity;

public class Galaxy
{
    public int Version { get; init; } // Primary Key
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    public ICollection<Star> Stars { get; init; } = [];
}