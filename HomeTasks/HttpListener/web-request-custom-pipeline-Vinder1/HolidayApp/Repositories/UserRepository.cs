using HolidayApp.Dto;
using HolidayApp.Entities;
using Npgsql;
using SapphireOrm;

namespace HolidayApp.Repositories;

public sealed class UserRepository(NpgsqlExecutor executor) : BaseRepository(executor)
{
    public async Task<int?> CreateUserAsync(SignUpDto info, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(info.Password).Split(";");
        var sql = SqlQuery.InsertValues<User>().Returning("userId").ToSql();        

        try
        {
            var result = await Executor.ExecuteReturningScalar(sql, ct, args: [
                ("login", info.Login),
                ("password_hash", hashPass[0]),
                ("password_salt", hashPass[1]),
                ("username", info.Username),
                ("coins", 10), ("role", "user")
            ]);
            return result is int id ? id : null;
        }
        catch (PostgresException e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    private static string? getByLoginSql;
    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct)
    {
        getByLoginSql ??= SqlQuery.SelectFrom<User>().By("login").ToSql();

        return await Executor.ExecuteReturningSingle<User>(getByLoginSql, ct, args: [
            ("login", login)
        ]);
    }

    private static string? getByIdSql;
    public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    {
        getByIdSql ??= SqlQuery.SelectFrom<User>().By("userId").ToSql();

        return await Executor.ExecuteReturningSingle<User>(getByIdSql, ct, args: [
            ("userId", id)
        ]);
    }

    private static string? getPublicSql;
    /// <summary>
    /// This method should be used when this record is returned to frontend. 
    /// Same as getByIdAsync, but doesn't give password info.
    /// </summary>
    public async Task<User?> GetPublicInformationByIdAsync(int id, CancellationToken ct)
    {
        // var sql = @"select userId, login, username, coins, role from holiday.User where userId=@userId;";
        getPublicSql ??= SqlQuery.SelectFrom<User>("userId", "login", "username", "coins", "role").By("userId").ToSql(); //короче не стало :D

        return await Executor.ExecuteReturningSingle<User>(getPublicSql, ct, args: [
            ("userId", id)
        ]);
    }

    private static string? updateSql;
    public async Task<bool> ForceUpdateUserAsync(int id, UpdateUserDto info, CancellationToken ct)
    {
        var hashPass = PasswordHasher.GetHash(info.Password).Split(";");

        // var sql =
        // """
        // UPDATE holiday.User
        // SET 
        // password_hash = @hash,
        // password_salt = @salt,
        // username = @username
        // WHERE userId = @id
        // RETURNING userId;
        // """;
        updateSql ??= SqlQuery.UpdateSet<User>("password_hash", "password_salt", "username")
            .By("userId").Returning("userId").ToSql();

        try
        {
            var result = await Executor.ExecuteReturningScalar(updateSql, ct, args: [
                ("userId", id),
                ("password_hash", hashPass[0]),
                ("password_salt", hashPass[1]),
                ("username", info.Username)
            ]);
            return result is int;
        }
        catch
        {
            return false;
        }
    }

    private static string? deleteSql;
    public async Task<int?> DeleteUserAsync(int id, CancellationToken ct)
    {
        // var sql =
        // """
        // DELETE FROM holiday.User
        // WHERE userId = @id
        // RETURNING userId;
        // """;
        deleteSql ??= SqlQuery.DeleteFrom<User>().By("userId").Returning("userId").ToSql();

        var result = await Executor.ExecuteReturningScalar(deleteSql, ct, args: [
            ("userId", id)
        ]);
        return result is int dId ? dId : null;
    }
}
