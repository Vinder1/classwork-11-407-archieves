using Emerald.Exceptions;
namespace Emerald;

public class DatabaseConnectionProvider
{
    private readonly string _connectionString;

    public DatabaseConnectionProvider(Configuration config)
    {
        if (config.TryGetString("DbConnection", out var s))
        {
            _connectionString = s;
        } else
        {
            throw new InitializationException("В config.json не найдена строка DbConnection");
        }
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }
}
