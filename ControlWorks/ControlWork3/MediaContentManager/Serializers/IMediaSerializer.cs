namespace MediaContentManager;

public interface IMediaSerializer<TMedia>
{
    void Serialize(Stream stream, TMedia items);
    TMedia Deserialize(Stream stream);
}