using Emerald;

namespace HolidayApp.Dto
{
    public class AuthDto : IAuthDto
    {
        public required int Id { get; init; }
        public required string Role { get; init; }
    }
}