using Emerald;
using ControlWorkApp.Dto;
using ControlWorkApp.Entities;
using Npgsql;
using ControlWorkApp.Services;

namespace ControlWorkApp.Repositories;

public sealed class TestRepository(DatabaseConnectionProvider provider)
{
    private readonly string _cs = provider.GetConnectionString();

    public async Task<int?> CreateTestAsync(NewTestDto info, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into library.Tests(title, description, authorId)
        values(@title, @desc, @author)
        returning id;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("title", info.Title);
        cmd.Parameters.AddWithValue("desc", info.Description ?? "-");
        cmd.Parameters.AddWithValue("author", info.AuthorId);

        try
        {
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is int session ? session : null;
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private const string SelectBase =
        "select id, title, description, authorId, createdAt" +
        " from library.Tests";

    public async Task<IReadOnlyList<Test>> GetAllAsync(CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + ";";
        await using var cmd = new NpgsqlCommand(sql, conn);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<Test> l = [];
        while (await rd.ReadAsync(ct))
        {
            var t = new Test
            {
                Id = rd.GetInt32(0),
                Title = rd.GetString(1),
                Description = rd.GetString(2),
                AuthorId = rd.GetInt32(3),
                CreatedAt = rd.GetDateTime(4)
            };
            l.Add(t);
        }

        return l.AsReadOnly();
    }

    public async Task<IReadOnlyList<Test>> GetByAuthorAsync(int authorId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where authorId = @id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", authorId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<Test> l = [];
        while (await rd.ReadAsync(ct))
        {
            var t = new Test
            {
                Id = rd.GetInt32(0),
                Title = rd.GetString(1),
                Description = rd.GetString(2),
                AuthorId = rd.GetInt32(3),
                CreatedAt = rd.GetDateTime(4)
            };
            l.Add(t);
        }

        return l.AsReadOnly();
    }
}
