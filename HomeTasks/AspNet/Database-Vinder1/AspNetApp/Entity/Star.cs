namespace AspNetApp.Entity;

public readonly struct Star
{
    public Star() {}

    public int Radius { get; init; }
    // public double Angle { get; init; }
    public int AngleMilliradians  { get; init; } // 0-6283
    
    public ICollection<SpaceshipStarVisit> VisitedByShips { get; init; } = [];
}