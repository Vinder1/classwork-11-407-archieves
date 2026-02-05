using Npgsql;

namespace HolidayHttpListener.Repositories;

public sealed class UserRepository
{
    private readonly string _cs;
    public UserRepository(string cs) => _cs = cs;

    public async Task<int?> CreateUserAsync(string login, string password, string username, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(password).Split(";");
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into holiday.User(login, password_hash, password_salt, username, coins, role)
        values(@login, @hash, @salt, @username, 10, 'user')
        returning userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);
        cmd.Parameters.AddWithValue("hash", hashPass[0]);
        cmd.Parameters.AddWithValue("salt", hashPass[1]);
        cmd.Parameters.AddWithValue("username", username);

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

    public sealed class UserRow
    {
        public int Id { get; init; }
        public string Login { get; init; } = "";
        public string Hash { get; init; } = "";
        public string Salt { get; init; } = "";
        public string Username { get; init; } = "Аноним";
        public int Coins { get; init; }
        public string Role { get; init; } = "user";
    }

    public async Task<UserRow?> GetByLoginAsync(string login, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select userId, login, password_hash, password_salt, username, coins, role from holiday.User where login=@login;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        if (await rd.ReadAsync(ct))
        {
            return new UserRow
            {
                Id = rd.GetInt32(0),
                Login = rd.GetString(1),
                Hash = rd.GetString(2),
                Salt = rd.GetString(3),
                Username = rd.GetString(4),
                Coins = rd.GetInt32(5),
                Role = rd.GetString(6)
            };
        }
        return null;
    }

    public async Task<UserRow?> GetByIdAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select userId, login, password_hash, password_salt, username, coins, role from holiday.User where userId=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        if (await rd.ReadAsync(ct))
        {
            return new UserRow
            {
                Id = rd.GetInt32(0),
                Login = rd.GetString(1),
                Hash = rd.GetString(2),
                Salt = rd.GetString(3),
                Username = rd.GetString(4),
                Coins = rd.GetInt32(5),
                Role = rd.GetString(6)
            };
        }
        return null;
    }

    public async Task<int?> ForceUpdateUserAsync(string login, string password, string username, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(password).Split(";");
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        UPDATE holiday.User
        SET 
        password_hash = @hash,
        password_salt = @salt,
        username = @username
        WHERE login = @login
        RETURNING userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);
        cmd.Parameters.AddWithValue("hash", hashPass[0]);
        cmd.Parameters.AddWithValue("salt", hashPass[1]);
        cmd.Parameters.AddWithValue("username", username);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int id ? id : null;
    }

    public async Task<int?> DeleteUserAsync(string login, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        DELETE FROM holiday.User
        WHERE login = @login
        RETURNING userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int dId ? dId : null;
    }
}
