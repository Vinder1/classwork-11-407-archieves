using System.Data;
using Npgsql;

namespace HolidayHttpListener.Repositories;

public sealed class PresentRepository
{
    private readonly string _cs;
    public PresentRepository(string cs) => _cs = cs;

    public async Task<bool> CanMakePresent(string login, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        const string sql = "SELECT holiday.check_balance(@login);";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result as bool? ?? false;
    }

    const string reduce = "UPDATE holiday.User SET coins = coins - 5 WHERE userId = @fromId;";
    const string add = "UPDATE holiday.User SET coins = coins + 5 WHERE userId = @toId;";

    public async Task<int?> CreatePresentAndTransferMoneyAsync(int fromId, int toId, int hid, CancellationToken ct, string inside = "-")
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        await using var transaction = await conn.BeginTransactionAsync(ct);

        try
        {
            await using var reduceCmd = new NpgsqlCommand(reduce, conn, transaction);
            reduceCmd.Parameters.AddWithValue("fromId", fromId);
            await reduceCmd.ExecuteNonQueryAsync(ct);

            await using var addCmd = new NpgsqlCommand(add, conn, transaction);
            addCmd.Parameters.AddWithValue("toId", toId);
            await addCmd.ExecuteNonQueryAsync(ct);

            const string sql =
            """
            insert into holiday.Present(fromid, toid, hid, inside)
            values(@from, @to, @holiday, @inside)
            returning presentId;
            """;

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("from", fromId);
            cmd.Parameters.AddWithValue("to", toId);
            cmd.Parameters.AddWithValue("holiday", hid);
            cmd.Parameters.AddWithValue("inside", inside);

            var result = await cmd.ExecuteScalarAsync(ct);

            await transaction.CommitAsync(ct);
            return result is int id ? id : null;
        }
        catch
        {
            await transaction.RollbackAsync();
            return null;
        }
    }

    public sealed class PresentRow
    {
        public int Id { get; init; }
        public int FromId { get; init; }
        public int ToId { get; init; }
        public int HolidayId { get; init; }
        public string Inside { get; init; } = null!;
    }

    public async Task<IReadOnlyList<PresentRow>> GetByReceiverId(int toId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select presentid, fromid, toid, hid, inside from holiday.Present where toid=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", toId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<PresentRow> presents = [];
        while (await rd.ReadAsync(ct))
        {
            presents.Add(new PresentRow
            {
                Id = rd.GetInt32(0),
                FromId = rd.GetInt32(1),
                ToId = rd.GetInt32(2),
                HolidayId = rd.GetInt32(3),
                Inside = rd.GetString(4),
            });
        }
        return presents.AsReadOnly();
    }

    public async Task<PresentRow?> GetById(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select presentid, fromid, toid, hid, inside from holiday.Present where presentid=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<PresentRow> holidays = [];
        if (await rd.ReadAsync(ct))
        {
            return new PresentRow
            {
                Id = rd.GetInt32(0),
                FromId = rd.GetInt32(1),
                ToId = rd.GetInt32(2),
                HolidayId = rd.GetInt32(3),
                Inside = rd.GetString(4),
            };
        }
        return null;
    }

    public async Task<int?> ForceUpdatePresentAsync(int id, string inside, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        if (!inside.StartsWith("[изменено] "))
        {
            inside = "[изменено] " + inside;
        }

        var sql =
        """
        UPDATE holiday.Present
        SET 
        inside = @inside
        WHERE presentId = @id
        RETURNING presentId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("inside", inside);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int uId ? uId : null;
    }

    public async Task<int?> DeletePresentAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        DELETE FROM holiday.Present
        WHERE presentId = @id
        RETURNING presentId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int dId ? dId : null;
    }
}
