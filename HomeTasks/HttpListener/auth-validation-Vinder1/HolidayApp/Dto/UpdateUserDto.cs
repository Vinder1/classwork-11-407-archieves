namespace HolidayApp.Dto
{
    public class UpdateUserDto
    {
        public required string Username { get; init; } = null!;
        public required string Password { get; init; } = null!;
        public required string Email { get; init; } = null!;
        public DateTime Birthday { get; init; }
    }
}