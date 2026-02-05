using SapphireOrm.Entities;

namespace HolidayApp.Entities
{
    [Entity("holiday.Holiday")]
    public sealed class Holiday
    {
        [BindSerial("holidayId")] public int Id { get; init; }
        [Bind("creator")] public int Creator { get; init; }
        [Bind("celebDay")] public DateTime CelebrationDay { get; init; }
        [Bind("name")] public string Name { get; init; } = null!;
    }
}