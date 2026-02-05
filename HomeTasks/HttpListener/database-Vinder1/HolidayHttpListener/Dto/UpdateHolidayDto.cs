namespace HolidayHttpListener.Dto
{
    public class UpdateHolidayDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public DateTime Date { get; init; }
    }
}