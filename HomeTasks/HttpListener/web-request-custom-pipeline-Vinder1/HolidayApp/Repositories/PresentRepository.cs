using Emerald;
using HolidayApp.Entities;
using Npgsql;
using SapphireOrm;

namespace HolidayApp.Repositories;

public sealed class PresentRepository(NpgsqlExecutor executor) : BaseRepository(executor)
{
    public async Task<bool> CanMakePresent(int id, CancellationToken ct)
    {
        var result = await Executor.ExecuteReturningScalar("SELECT holiday.check_balance(@id);", ct,
            args: [("id", id)]
        );
        return result as bool? ?? false;
    }

    const string reduce = "UPDATE holiday.User SET coins = coins - 5 WHERE userId = @fromId;";
    const string add = "UPDATE holiday.User SET coins = coins + 5 WHERE userId = @toId;";

    public async Task<int?> CreatePresentAndTransferMoneyAsync(int fromId, int toId, int hid, CancellationToken ct, string inside = "-")
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);

        await using var transaction = await conn.BeginTransactionAsync(ct);

        try
        {
            if (!await CanMakePresent(fromId, ct))
            {
                return null;
            }

            await using var reduceCmd = new NpgsqlCommand(reduce, conn, transaction);
            reduceCmd.Parameters.AddWithValue("fromId", fromId);
            await reduceCmd.ExecuteNonQueryAsync(ct);

            await using var addCmd = new NpgsqlCommand(add, conn, transaction);
            addCmd.Parameters.AddWithValue("toId", toId);
            await addCmd.ExecuteNonQueryAsync(ct);

            var sql = SqlQuery.InsertValues<Present>().Returning("presentId").ToSql();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("fromId", fromId);
            cmd.Parameters.AddWithValue("toId", toId);
            cmd.Parameters.AddWithValue("hId", hid);
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

    public async Task<IReadOnlyList<Present>> GetByReceiverId(int toId, CancellationToken ct)
    {
        var sql = SqlQuery.SelectFrom<Present>().By("toId").ToSql();

        return await Executor.ExecuteReturningMultiple<Present>(sql, ct,
            args: [("toId", toId)]
        );
    }
}
