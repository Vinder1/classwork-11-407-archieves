using Emerald;
using HolidayApp.Dto;
using HolidayApp.Entities;
using Npgsql;

namespace HolidayApp.Repositories;

public sealed class UserRepository(DatabaseConnectionProvider provider)
{
    private readonly string _cs = provider.GetConnectionString();

    public async Task<int?> CreateUserAsync(SignUpDto info, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(info.Password).Split(";");
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into holiday.User(login, password_hash, password_salt, username, email, coins, role, birthday)
        values(@login, @hash, @salt, @username, @email, 10, 'user', @birthday)
        returning userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", info.Login);
        cmd.Parameters.AddWithValue("hash", hashPass[0]);
        cmd.Parameters.AddWithValue("salt", hashPass[1]);
        cmd.Parameters.AddWithValue("username", info.Username);
        cmd.Parameters.AddWithValue("email", info.Email);
        cmd.Parameters.AddWithValue("birthday", info.Birthday);
        //no, i will not even add AUF to db

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

    private const string SelectBase =
        "select userId, login, password_hash, password_salt, username, email, coins, role, birthday" +
        " from holiday.User";

    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where login=@login;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", login);

        await using var rd = await cmd.ExecuteReaderAsync(ct);

        if (await rd.ReadAsync(ct))
        {
            return new User
            {
                Id = rd.GetInt32(0),
                Login = rd.GetString(1),
                Hash = rd.GetString(2),
                Salt = rd.GetString(3),
                Username = rd.GetString(4),
                Email = rd.GetString(5),
                Coins = rd.GetInt32(6),
                Role = rd.GetString(7),
                BirthDay = rd.GetDateTime(8)
            };
        }

        return null;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where userId=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        await using var rd = await cmd.ExecuteReaderAsync(ct);
        if (await rd.ReadAsync(ct))
        {
            return new User
            {
                Id = rd.GetInt32(0),
                Login = rd.GetString(1),
                Hash = rd.GetString(2),
                Salt = rd.GetString(3),
                Username = rd.GetString(4),
                Email = rd.GetString(5),
                Coins = rd.GetInt32(6),
                Role = rd.GetString(7),
                BirthDay = rd.GetDateTime(8)
            };
        }
        return null;
    }

    /// <summary>
    /// This method should be used when this record is returned to frontend. 
    /// Same as getByIdAsync, but doesn't give password info.
    /// </summary>
    public async Task<User?> GetPublicInformationByIdAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = @"select userId, login, username, coins, role, email, birthday from holiday.User where userId=@id;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        try
        {
            await using var rd = await cmd.ExecuteReaderAsync(ct);
            if (await rd.ReadAsync(ct))
            {
                return new User
                {
                    Id = rd.GetInt32(0),
                    Login = rd.GetString(1),
                    Username = rd.GetString(2),
                    Coins = rd.GetInt32(3),
                    Role = rd.GetString(4),
                    Email = rd.GetString(5),
                    BirthDay = rd.GetDateTime(6)
                };
            }
        } catch {}
        return null;
    }

    public async Task<bool> ForceUpdateUserAsync(int id, UpdateUserDto info, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(info.Password).Split(";");
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        UPDATE holiday.User
        SET 
        password_hash = @hash,
        password_salt = @salt,
        username = @username,
        email = @email
        WHERE userId = @id
        RETURNING userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);
        cmd.Parameters.AddWithValue("hash", hashPass[0]);
        cmd.Parameters.AddWithValue("salt", hashPass[1]);
        cmd.Parameters.AddWithValue("username", info.Username);
        cmd.Parameters.AddWithValue("email", info.Email);
        cmd.Parameters.AddWithValue("birthday", info.Birthday);

        try
        {
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is int;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int?> DeleteUserAsync(int id, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        DELETE FROM holiday.User
        WHERE userId = @id
        RETURNING userId;
        """;
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("id", id);

        var result = await cmd.ExecuteScalarAsync(ct);
        return result is int dId ? dId : null;
    }
}
