using Emerald;
using ControlWorkApp.Dto;
using ControlWorkApp.Entities;
using Npgsql;

namespace ControlWorkApp.Repositories;

public sealed class AnswerRepository(DatabaseConnectionProvider provider)
{
    private readonly string _cs = provider.GetConnectionString();

    public async Task<int?> CreateAnswerAsync(NewAnswerDto info, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into library.Answers(questionId, studentId, givenAnswer, isCorrect)
        values(@questionId, @studentId, @givenAnswer, @isCorrect)
        returning id;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("questionId", info.QuestionId);
        cmd.Parameters.AddWithValue("studentId", info.StudentId);
        cmd.Parameters.AddWithValue("givenAnswer", info.GivenAnswer);
        cmd.Parameters.AddWithValue("isCorrect", info.IsCorrect);

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
        "select id, questionId, studentId, givenAnswer, isCorrect" +
        " from library.Answers";

    public async Task<IReadOnlyList<Answer>> GetByUserAsync(int studentId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where studentId = @id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", studentId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<Answer> l = [];
        while (await rd.ReadAsync(ct))
        {
            var q = new Answer
            {
                Id = rd.GetInt32(0),
                QuestionId = rd.GetInt32(1),
                StudentId = rd.GetInt32(2),
                GivenAnswer = rd.GetString(3),
                IsCorrect = rd.GetBoolean(4)
            };
            l.Add(q);
        }

        return l.AsReadOnly();
    }

    public async Task<IReadOnlyList<TestWithTries>> TryGetStatistics(int teacherId, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"
            SELECT 
                t.id, 
                SUM(CASE WHEN a.isCorrect THEN 1 ELSE 0 END) AS correct, 
                COUNT(*) AS overall
            FROM library.Tests t 
            JOIN library.Questions q ON q.testId = t.id
            JOIN library.Answers a ON a.questionId = q.id
            WHERE t.authorId = @authorId
            GROUP BY t.id";
        
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("authorId", teacherId);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        List<TestWithTries> l = [];
        while (await rd.ReadAsync(ct))
        {
            var q = new TestWithTries
            {
                TestId = rd.GetInt32(0),
                Correct = rd.GetInt32(1),
                Overall = rd.GetInt32(2)
            };
            l.Add(q);
        }

        return l.AsReadOnly();
    }
}
