namespace AspNetApp.Entity;

public class Spaceship
{
    public Guid OwnerId { get; init; }
    public int GalaxyVersion { get; init; }
    
    public int LocatedRadius { get; set; }
    public int LocatedAngleMilliradians { get; set; }
    
    public Player Owner { get; init; } = null!;
    
    public ICollection<SpaceshipStarVisit> VisitedStars { get; init; } = [];
}