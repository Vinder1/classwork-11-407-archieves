namespace AspNetApp.Repository.DbEntity;

public class Star
{
    // Composite Key (gv,r,a)
    public int GalaxyVersion { get; init; } // FK к Galaxy
    public int Radius { get; init; }
    public int AngleMilliradians  { get; init; } // 0-6283
    
    public Galaxy Galaxy { get; init; } = null!;
    
    public ICollection<SpaceshipStarVisit> VisitedByShips { get; init; } = [];
}