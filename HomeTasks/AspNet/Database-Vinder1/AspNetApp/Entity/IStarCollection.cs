namespace AspNetApp.Entity;

public interface IStarCollection
{
    public Star[][] Stars { get; }
    public Star? Find(int radius, int angleMilliradians);
    public IEnumerable<Star> GetAllStars();
    public IEnumerable<Star> FindNearbyStars(int sourceRadius, int sourceAngleMilliradians, int radius);
}