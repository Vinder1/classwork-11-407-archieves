using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MediaContentManager;

public class YamlMediaSerializer<TMedia> : IMediaSerializer<TMedia>
{
    private ISerializer serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
    private IDeserializer deserializer = new DeserializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();
    
    public void Serialize(Stream stream, TMedia items)
    {
        var writer = new StreamWriter(stream);
        serializer.Serialize(writer, items);
    }

    public TMedia Deserialize(Stream stream)
    {
        var reader = new StreamReader(stream);
        return deserializer.Deserialize<TMedia>(reader);
    }
}