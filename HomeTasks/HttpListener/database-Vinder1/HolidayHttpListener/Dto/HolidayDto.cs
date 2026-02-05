namespace HolidayHttpListener.Dto
{
    public class HolidayDto
    {
        public string Name { get; init; } = null!;
        public DateTime Date { get; init; }
        public string CreatorLogin { get; init; } = null!;
    }
}