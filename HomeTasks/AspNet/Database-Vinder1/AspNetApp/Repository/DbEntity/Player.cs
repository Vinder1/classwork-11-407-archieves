namespace AspNetApp.Repository.DbEntity;

public class Player
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public DateTime RegisteredAt { get; init; } = DateTime.UtcNow;

    public Spaceship? Spaceship { get; set; }
}