using System.Text.Json;

namespace HolidayHttpListener.Repositories;

public class DatabaseConnectionProvider
{
    private ConnectionParamsDto Parameters { get; init; }

    public DatabaseConnectionProvider(string configFilePath)
    {
        var text = "";
        try
        {
            text = File.ReadAllText(configFilePath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Файл {configFilePath} не найден");            
        }
        
        Parameters = JsonSerializer.Deserialize<ConnectionParamsDto>(text);
    }

    public string GetConnectionString()
    {
        return $"Host={Parameters.Host};Username={Parameters.Username};Password={Parameters.Password};Database={Parameters.Database}";
    }

    private readonly struct ConnectionParamsDto
    {
        public string Host { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string Database { get; init; }
    }
}
