namespace AspNetApp.Entity;

public class Galaxy
{
    public int Version { get; init; }
    public IStarCollection Stars { get; init; } = null!; 
}