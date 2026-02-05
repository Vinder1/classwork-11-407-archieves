namespace ControlWorkApp.Entities
{
    public sealed class User
    {
        public int Id { get; init; }
        public string Login { get; init; } = "";
        public string Hash { get; init; } = "";
        public string Salt { get; init; } = "";
        public required string FullName { get; init; } = null!;
        public string Role { get; init; } = null!;
        public string SessionId { get; init; } = null!;
    }
}