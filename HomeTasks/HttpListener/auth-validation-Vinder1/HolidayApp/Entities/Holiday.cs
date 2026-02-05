namespace HolidayApp.Entities
{
    public sealed class Holiday
    {
        public int Id { get; init; }
        public int Creator { get; init; }
        public DateTime CelebrationDay { get; init; }
        public string Name { get; init; } = null!;
    }
}