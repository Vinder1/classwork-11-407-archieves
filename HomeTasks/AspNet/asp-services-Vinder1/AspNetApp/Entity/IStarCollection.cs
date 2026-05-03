namespace AspNetApp.Entity;

public interface IStarCollection
{
    public Star[][] Stars { get; }
    public Star? Find(int radius, double angle);
    public IEnumerable<Star> GetAllStars();
    public IEnumerable<Star> FindNearbyStars(int sourceRadius, double sourceAngle, int radius);
}