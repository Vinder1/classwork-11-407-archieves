using System.Data;
using Emerald;
using HolidayApp.Entities;
using Npgsql;
using SapphireOrm;

namespace HolidayApp.Repositories;

public sealed class HolidayRepository(NpgsqlExecutor executor) : BaseRepository(executor)
{
    private static string? createSql;
    public async Task<int?> CreateHolidayAsync(int creator, string name, DateTime celebDay, CancellationToken ct)
    {
        // var sql =
        // """
        // insert into holiday.Holiday(creator, celebDay, name)
        // values(@creator, @celebDay, @name)
        // returning holidayId;
        // """;
        createSql ??= SqlQuery.InsertValues<Holiday>().Returning("holidayId").ToSql();

        try
        {
            var result = await Executor.ExecuteReturningScalar(createSql, ct,
                args: [("creator", creator), ("celebDay", celebDay), ("name", name)]
            );
            return result is int id ? id : null;
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    private static string? getByCreatorSql;
    public async Task<IReadOnlyList<Holiday>> GetByCreatorId(int creatorId, CancellationToken ct)
    {
        getByCreatorSql ??= SqlQuery.SelectFrom<Holiday>().By("creator").ToSql();

        return await Executor.ExecuteReturningMultiple<Holiday>(getByCreatorSql, ct,
            args: [("creator", creatorId)]
        );
    }

    private static string? getByIdSql;
    public async Task<Holiday?> GetById(int id, CancellationToken ct)
    {
        getByIdSql ??= SqlQuery.SelectFrom<Holiday>().By("holidayid").ToSql();

        return await Executor.ExecuteReturningSingle<Holiday>(getByIdSql, ct,
            args: [("holidayid", id)]
        );
    }

    private static string? updateSql;
    public async Task<int?> ForceUpdateHolidayAsync(int id, string name, DateTime celebDay, CancellationToken ct)
    {
        // var sql =
        // """
        // UPDATE holiday.Holiday
        // SET 
        // name = @name,
        // celebDay = @celebDay
        // WHERE holidayId = @holidayId
        // RETURNING holidayId;
        // """;
        updateSql ??= SqlQuery.UpdateSet<Holiday>("name", "celebDay").By("holidayId").Returning("holidayId").ToSql();

        var result = await Executor.ExecuteReturningScalar(updateSql, ct,
            args: [("holidayid", id), ("celebDay", celebDay), ("name", name)]
        );

        return result is int uId ? uId : null;
    }


    public async Task DeleteHolidayAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand("holiday.delete_holiday", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("holiday_id", id);

        await cmd.ExecuteScalarAsync(ct);
    }
}
