namespace HolidayHttpListener.Dto
{
    public class PresentDto
    {
        public string? Inside { get; init; }
        public required string SenderLogin { get; init; } = null!;
        public required string ReceiverLogin { get; init; } = null!;
        public required int HolidayId { get; init; }
    }
}