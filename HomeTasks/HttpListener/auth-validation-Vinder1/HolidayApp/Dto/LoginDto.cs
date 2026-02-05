namespace HolidayApp.Dto
{
    public class LoginDto
    {
        public required string Login { get; init; } = null!;
        public required string Password { get; init; } = null!;
    }
}