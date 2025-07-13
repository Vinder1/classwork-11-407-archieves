namespace MediaContentManager;

public interface IMediaSerializerFactory<TMedia>
{
    IMediaSerializer<TMedia> GetMediaSerializer(string path);
}