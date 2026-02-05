namespace HolidayApp.Entities
{
    public sealed class Present
    {
        public int Id { get; init; }
        public int FromId { get; init; }
        public int ToId { get; init; }
        public int HolidayId { get; init; }
        public string Inside { get; init; } = null!;
    }
}