namespace AspNetApp.Entity;

public readonly struct SpaceshipStarVisit()
{
    public DateTime VisitedAt { get; init; } = DateTime.UtcNow;
    public int GalaxyVersion { get; init; }
    
    public Spaceship Spaceship { get; init; } = null!;
    public Star Star { get; init; } = default;
}