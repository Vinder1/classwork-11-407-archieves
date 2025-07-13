namespace MediaContentManager;

public class AudioTrack : IMediaItem
{
    public required string Title { get; init; }
    public required TimeSpan Duration { get; init; }
    public required List<string> Tags { get; init; }
    public required Dictionary<string, object> Metadata { get; init; }
    
    public required string Path { get; init; }
    
    public required int Bitrate { get; init; }
}