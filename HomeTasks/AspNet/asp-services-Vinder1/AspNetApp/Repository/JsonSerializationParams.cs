using System.Text.Json;

namespace AspNetApp.Repository;

public static class JsonSerializationParams
{
    public static JsonSerializerOptions Options { get; } = new()
    { 
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
}