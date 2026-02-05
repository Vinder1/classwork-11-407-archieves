namespace ControlWorkApp.Entities
{
    public sealed class Test
    {
        public required int Id { get; init; }
        public required string Title { get; init; } = null!;
        public required string Description { get; init; }
        public required int AuthorId { get; init; }
        public required DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }
}