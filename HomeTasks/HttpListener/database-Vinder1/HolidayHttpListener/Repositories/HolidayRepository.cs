using System.Data;
using System.Security.Cryptography.X509Certificates;
using Npgsql;

namespace HolidayHttpListener.Repositories;

public sealed class HolidayRepository
{
    private readonly string _cs;
    public HolidayRepository(string cs) => _cs = cs;

    public async Task<int?> CreateHolidayAsync(int creator, string name, DateTime celebDay, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into holiday.Holiday(creator, celebDay, name)
        values(@creator, @celebDay, @name)
        returning holidayId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("creator", creator);
        cmd.Parameters.AddWithValue("celebDay", celebDay);
        cmd.Parameters.AddWithValue("name", name);

        try
        {
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is int id ? id : null;
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    public sealed class HolidayRow
    {
        public int Id { get; init; }
        public int Creator { get; init; }
        public DateTime CelebrationDay { get; init; }
        public string Name { get; init; } = null!;
    }

    public async Task<IReadOnlyList<HolidayRow>> GetByCreatorId(int creatorId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select holidayid, creator, celebday, name from holiday.Holiday where creator=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", creatorId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<HolidayRow> holidays = [];
        while (await rd.ReadAsync(ct))
        {
            holidays.Add(new HolidayRow
            {
                Id = rd.GetInt32(0),
                Creator = rd.GetInt32(1),
                CelebrationDay = rd.GetDateTime(2),
                Name = rd.GetString(3),
            });
        }
        return holidays.AsReadOnly();
    }

    public async Task<HolidayRow?> GetById(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select holidayid, creator, celebday, name from holiday.Holiday where holidayid=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<HolidayRow> holidays = [];
        if (await rd.ReadAsync(ct))
        {
            return new HolidayRow
            {
                Id = rd.GetInt32(0),
                Creator = rd.GetInt32(1),
                CelebrationDay = rd.GetDateTime(2),
                Name = rd.GetString(3),
            };
        }
        return null;
    }

    public async Task<int?> ForceUpdateHolidayAsync(int id, string name, DateTime celebDay, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        UPDATE holiday.Holiday
        SET 
        name = @name,
        celebDay = @date
        WHERE holidayId = @id
        RETURNING holidayId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("date", celebDay);
        cmd.Parameters.AddWithValue("name", name);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int uId ? uId : null;
    }

    public async Task DeleteHolidayAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand("holiday.delete_holiday", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("holiday_id", id);

        await cmd.ExecuteScalarAsync(ct);
    }
}
