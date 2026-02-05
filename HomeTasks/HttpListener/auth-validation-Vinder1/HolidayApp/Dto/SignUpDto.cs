namespace HolidayApp.Dto
{
    public class SignUpDto
    {
        public required string Login { get; init; } = null!;
        public required string Username { get; init; } = null!;
        public required string Password { get; init; } = null!;
        public required string Email { get; init; } = null!;
        public DateTime Birthday { get; init; }
        public int Auf { get; init; } // Absolutely Useless Field
    }
}