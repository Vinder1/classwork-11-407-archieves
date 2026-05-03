namespace AspNetApp.Repository.DbEntity;

public class SpaceshipStarVisit
{
    public Guid SpaceshipId { get; init; }
    public int StarGalaxyVersion { get; init; }
    public int StarRadius { get; init; }
    public int StarAngleMilliradians { get; init; }
    
    public DateTime VisitedAt { get; init; } = DateTime.UtcNow;

    public Spaceship Spaceship { get; init; } = null!;
    public Star Star { get; init; } = null!;
}