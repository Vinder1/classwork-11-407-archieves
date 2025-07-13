namespace MediaContentManager;

public class MediaSerializerFactory<TMedia> : IMediaSerializerFactory<TMedia>
{
    public class UnsupportedExtensionException : Exception {}
    
    public IMediaSerializer<TMedia> GetMediaSerializer(string path)
    {
        return Path.GetExtension(path) switch
        {
            ".json" => new JsonMediaSerializer<TMedia>(),
            ".yaml" => new YamlMediaSerializer<TMedia>(),
            _ => throw new UnsupportedExtensionException()
        };
    }
}