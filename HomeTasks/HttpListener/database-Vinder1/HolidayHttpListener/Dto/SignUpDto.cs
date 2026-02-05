namespace HolidayHttpListener.Dto
{
    public class SignUpDto
    {
        public required string Login { get; init; } = null!;
        public required string Username { get; init; } = null!;
        public required string Password { get; init; } = null!;
    }
}