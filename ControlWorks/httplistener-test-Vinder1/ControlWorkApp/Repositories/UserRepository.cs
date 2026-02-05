using Emerald;
using ControlWorkApp.Dto;
using ControlWorkApp.Entities;
using Npgsql;
using ControlWorkApp.Services;

namespace ControlWorkApp.Repositories;

public sealed class UserRepository(DatabaseConnectionProvider provider)
{
    private readonly string _cs = provider.GetConnectionString();

    public async Task<string?> CreateUserAsync(SignUpDto info, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(info.Password);
        var sessionId = SessionAuth.GenerateSessionId();
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql =
        """
        insert into library.Users(login, passwordHash, sessionId, role, fullName)
        values(@login, @hash, @session, @role, @fullname)
        returning sessionId;
        """;

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("login", info.Login);
        cmd.Parameters.AddWithValue("hash", hashPass);
        cmd.Parameters.AddWithValue("fullname", info.FullName);
        cmd.Parameters.AddWithValue("session", sessionId);
        cmd.Parameters.AddWithValue("role", info.Role);

        try
        {
            var result = await cmd.ExecuteScalarAsync(ct);
            return result is string session ? session : null;
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    private const string SelectBase =
        "select id, login, passwordHash, role, fullName, sessionId" +
        " from library.Users";

    public async Task<User?> GetBySessionAsync(string session, CancellationToken ct)
    {
        await using var conn = new NpgsqlConnection(_cs);
        await conn.OpenAsync(ct);

        var sql = SelectBase + @" where sessionId=@session;";
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("session", session);

        await using var rd = await cmd.ExecuteReaderAsync(ct);

        if (await rd.ReadAsync(ct))
        {
            return new User
            {
                Id = rd.GetInt32(0),
                Login = rd.GetString(1),
                Hash = rd.GetString(2),
                Role = rd.GetString(3),
                FullName = rd.GetString(4),
                SessionId = rd.GetString(5),
            };
        }

        return null;
    }

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
                Role = rd.GetString(3),
                FullName = rd.GetString(4),
                SessionId = rd.GetString(5),
            };
        }

        return null;
    }

    // public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    // {
    //     await using var conn = new NpgsqlConnection(_cs);
    //     await conn.OpenAsync(ct);

    //     var sql = SelectBase + @" where userId=@id;";
    //     await using var cmd = new NpgsqlCommand(sql, conn);
    //     cmd.Parameters.AddWithValue("id", id);

    //     await using var rd = await cmd.ExecuteReaderAsync(ct);
    //     if (await rd.ReadAsync(ct))
    //     {
    //         return new User
    //         {
    //             Id = rd.GetInt32(0),
    //             Login = rd.GetString(1),
    //             Hash = rd.GetString(2),
    //             Salt = rd.GetString(3),
    //             Username = rd.GetString(4),
    //             Email = rd.GetString(5),
    //             Coins = rd.GetInt32(6),
    //             Role = rd.GetString(7),
    //             BirthDay = rd.GetDateTime(8)
    //         };
    //     }
    //     return null;
    // }

    /// <summary>
    /// This method should be used when this record is returned to frontend. 
    /// Same as getByIdAsync, but doesn't give password info.
    /// </summary>
    // public async Task<User?> GetPublicInformationByIdAsync(int id, CancellationToken ct)
    // {
    //     await using var conn = new NpgsqlConnection(_cs);
    //     await conn.OpenAsync(ct);

    //     var sql = @"select userId, login, username, coins, role, email, birthday from holiday.User where userId=@id;";
    //     await using var cmd = new NpgsqlCommand(sql, conn);
    //     cmd.Parameters.AddWithValue("id", id);

    //     try
    //     {
    //         await using var rd = await cmd.ExecuteReaderAsync(ct);
    //         if (await rd.ReadAsync(ct))
    //         {
    //             return new User
    //             {
    //                 Id = rd.GetInt32(0),
    //                 Login = rd.GetString(1),
    //                 Username = rd.GetString(2),
    //                 Coins = rd.GetInt32(3),
    //                 Role = rd.GetString(4),
    //                 Email = rd.GetString(5),
    //                 BirthDay = rd.GetDateTime(6)
    //             };
    //         }
    //     } catch {}
    //     return null;
    // }

    // public async Task<bool> ForceUpdateUserAsync(int id, UpdateUserDto info, CancellationToken ct)
    // {
    //     var hashPass = PasswordHasher.GetHash(info.Password).Split(";");
    //     await using var conn = new NpgsqlConnection(_cs);
    //     await conn.OpenAsync(ct);

    //     var sql =
    //     """
    //     UPDATE holiday.User
    //     SET 
    //     password_hash = @hash,
    //     password_salt = @salt,
    //     username = @username,
    //     email = @email
    //     WHERE userId = @id
    //     RETURNING userId;
    //     """;
    //     await using var cmd = new NpgsqlCommand(sql, conn);
    //     cmd.Parameters.AddWithValue("id", id);
    //     cmd.Parameters.AddWithValue("hash", hashPass[0]);
    //     cmd.Parameters.AddWithValue("salt", hashPass[1]);
    //     cmd.Parameters.AddWithValue("username", info.Username);
    //     cmd.Parameters.AddWithValue("email", info.Email);
    //     cmd.Parameters.AddWithValue("birthday", info.Birthday);

    //     try
    //     {
    //         var result = await cmd.ExecuteScalarAsync(ct);
    //         return result is int;
    //     }
    //     catch
    //     {
    //         return false;
    //     }
    // }

    // public async Task<int?> DeleteUserAsync(int id, CancellationToken ct)
    // {
    //     await using var conn = new NpgsqlConnection(_cs);
    //     await conn.OpenAsync(ct);

    //     var sql =
    //     """
    //     DELETE FROM holiday.User
    //     WHERE userId = @id
    //     RETURNING userId;
    //     """;
    //     await using var cmd = new NpgsqlCommand(sql, conn);
    //     cmd.Parameters.AddWithValue("id", id);

    //     var result = await cmd.ExecuteScalarAsync(ct);
    //     return result is int dId ? dId : null;
    // }
}
