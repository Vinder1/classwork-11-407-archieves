using SapphireOrm.Entities;

namespace HolidayApp.Entities
{
    [Entity("holiday.User")]
    public sealed class User
    {
        [BindSerial("userId")] public int Id { get; init; }
        [Bind("login")] public string Login { get; init; } = "";
        [Bind("password_hash")] public string Hash { get; init; } = "";
        [Bind("password_salt")] public string Salt { get; init; } = "";
        [Bind("username")] public string Username { get; init; } = "Аноним";
        [Bind("coins")] public int Coins { get; init; }
        [Bind("role")] public string Role { get; init; } = "user";
    }
}