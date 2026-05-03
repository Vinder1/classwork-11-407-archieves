namespace AspNetApp.Entity;

public class Spaceship
{
    public Guid OwnerId { get; init; }
    public int GalaxyVersion { get; init; }

    public int LocatedRadius { get; set; }
    public double LocatedAngle { get; set; }
}