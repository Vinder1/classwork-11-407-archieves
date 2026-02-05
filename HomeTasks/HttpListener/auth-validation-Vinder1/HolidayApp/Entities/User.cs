namespace HolidayApp.Entities
{
    public sealed class User
    {
        public int Id { get; init; }
        public string Login { get; init; } = "";
        public string Hash { get; init; } = "";
        public string Salt { get; init; } = "";
        public string Username { get; init; } = "Аноним";
        public int Coins { get; init; }
        public string Role { get; init; } = "user";
        public string Email { get; init; } = "";
        public DateTime BirthDay { get; init; }
    }
}