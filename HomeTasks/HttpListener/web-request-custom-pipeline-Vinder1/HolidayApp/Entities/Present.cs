using SapphireOrm.Entities;

namespace HolidayApp.Entities
{
    [Entity("holiday.Present")]
    public sealed class Present
    {
        [BindSerial("presentId")] public int Id { get; init; }
        [Bind("fromId")] public int FromId { get; init; }
        [Bind("toId")] public int ToId { get; init; }
        [Bind("hid")] public int HolidayId { get; init; }
        [Bind("inside")] public string Inside { get; init; } = null!;
    }
}