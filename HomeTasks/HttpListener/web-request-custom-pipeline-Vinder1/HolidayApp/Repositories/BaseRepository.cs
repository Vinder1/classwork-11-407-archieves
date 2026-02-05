namespace HolidayApp.Repositories;

public class BaseRepository(NpgsqlExecutor executor)
{
    protected NpgsqlExecutor Executor => executor;
    protected string ConnectionString => executor.ConnectionString;
}
