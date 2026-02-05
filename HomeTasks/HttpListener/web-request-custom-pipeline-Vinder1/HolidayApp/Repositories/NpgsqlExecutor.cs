using Emerald;
using Npgsql;
using SapphireOrm;

namespace HolidayApp.Repositories;

public class NpgsqlExecutor(DatabaseConnectionProvider provider)
{
    public string ConnectionString { get; } = provider.GetConnectionString();
    
    public async Task<IReadOnlyList<T>> ExecuteReturningMultiple<T>(string sql, CancellationToken ct, params (string param, object value)[] args)
    where T : new()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand(sql, conn);
        foreach (var (param, value) in args)
        {
            cmd.Parameters.AddWithValue(param, value);
        }

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        return await SqlReader.ReadMultiple<T>(rd);
    }

    public async Task<T?> ExecuteReturningSingle<T>(string sql, CancellationToken ct, (string param, object value)[] args)
    where T : new()
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand(sql, conn);
        foreach (var (param, value) in args)
        {
            cmd.Parameters.AddWithValue(param, value);
        }

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        return await SqlReader.ReadSingle<T>(rd);
    }

    public async Task<object?> ExecuteReturningScalar(string sql, CancellationToken ct, (string param, object value)[] args)
    {
        await using var conn = new NpgsqlConnection(ConnectionString);
        await conn.OpenAsync(ct);

        await using var cmd = new NpgsqlCommand(sql, conn);
        foreach (var (param, value) in args)
        {
            cmd.Parameters.AddWithValue(param, value);
        }

        return await cmd.ExecuteScalarAsync(ct);
    }
}
