using System.Text.Json;

namespace MediaContentManager;

public class JsonMediaSerializer<TMedia> : IMediaSerializer<TMedia>
{
    public void Serialize(Stream stream, TMedia items)
    {
        JsonSerializer.Serialize(items);
    }

    public TMedia Deserialize(Stream stream)
    {
        return JsonSerializer.Deserialize<TMedia>(stream)!;
    }
}