using AspNetApp.Entity;

namespace AspNetApp.Repository;

public class JsonGalaxy(int version, Star[][] stars)
{
    public int Version { get; init; } = version;
    public Star[][] Stars { get; init; } = stars; 
}