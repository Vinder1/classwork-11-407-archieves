namespace ControlWorkApp.Dto
{
    public class SignUpDto
    {
        public required string Login { get; init; } = null!;
        public required string Password { get; init; } = null!;
        public required string Role { get; init; } = null!;
        public required string FullName { get; init; } = null!;
    }
}