using System.Text.Json;

namespace Emerald;

public class Configuration
{
    private readonly JsonElement json;

    public Configuration(string path)
    {
        var text = File.ReadAllText(path);
        using JsonDocument doc = JsonDocument.Parse(text);
        json = doc.RootElement.Clone();
    }

    public bool TryGetString(string arg, out string value)
    {
        if (json.TryGetProperty(arg, out var jsonElement) && jsonElement.ValueKind == JsonValueKind.String)
        {
            value = jsonElement.GetString()!;
            return true;
        }
        value = "";
        return false;
    }
}
