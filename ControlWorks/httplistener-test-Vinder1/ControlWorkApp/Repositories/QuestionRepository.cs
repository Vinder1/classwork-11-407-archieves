using Emerald;
using ControlWorkApp.Dto;
using ControlWorkApp.Entities;
using Npgsql;

namespace ControlWorkApp.Repositories;

public sealed class QuestionRepository(DatabaseConnectionProvider provider)
{
    private readonly string _cs = provider.GetConnectionString();

    public async Task<int?> CreateQuestionAsync(NewQuestionDto info, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into library.Questions(testId, text, correctAnswer)
        values(@testId, @text, @correctAnswer)
        returning id;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("testId", info.TestId);
        cmd.Parameters.AddWithValue("text", info.Text);
        cmd.Parameters.AddWithValue("correctAnswer", info.CorrectAnswer);

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
        "select id, testId, text, correctAnswer" +
        " from library.Questions";

    public async Task<IReadOnlyList<Question>> GetByTestAsync(int testId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where testId = @id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", testId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<Question> l = [];
        while (await rd.ReadAsync(ct))
        {
            var t = new Question
            {
                Id = rd.GetInt32(0),
                TestId = rd.GetInt32(1),
                Text = rd.GetString(2),
                CorrectAnswer = rd.GetString(3)
            };
            l.Add(t);
        }

        return l.AsReadOnly();
    }

    public async Task<Question?> GetByIdAsync(int testId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where id = @id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", testId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        if (await rd.ReadAsync(ct))
        {
            return new Question
            {
                Id = rd.GetInt32(0),
                TestId = rd.GetInt32(1),
                Text = rd.GetString(2),
                CorrectAnswer = rd.GetString(3)
            };
        }

        return null;
    }
}
