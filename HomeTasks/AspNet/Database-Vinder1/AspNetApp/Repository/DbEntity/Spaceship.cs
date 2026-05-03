namespace AspNetApp.Repository.DbEntity;

public class Spaceship
{
    public Guid OwnerId { get; init; } // FK к Player
    public int GalaxyVersion { get; init; } // FK к Galaxy
    
    public int LocatedRadius { get; set; }
    public int LocatedAngleMilliradians { get; set; }
    
    public Player Owner { get; init; } = null!;
    public Galaxy Galaxy { get; init; } = null!;
    
    public ICollection<SpaceshipStarVisit> VisitedStars { get; init; } = [];
}