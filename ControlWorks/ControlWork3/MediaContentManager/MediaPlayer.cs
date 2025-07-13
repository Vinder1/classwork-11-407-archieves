namespace MediaContentManager;

public class MediaPlayer
{
    private IPlaybackState State = new StoppedState();
    public event EventHandler<IPlaybackState> OnStateChanged = (_,_) => { };
    
    
    public IMediaItem? MediaItem { get; private set; } = null!;

    public void SetMediaItem(IMediaItem? mediaItem)
    {
        Stop();
        MediaItem = mediaItem;
    }

    public void ChangeState(IPlaybackState state)
    {
        State = state;
        OnStateChanged.Invoke(this, State);
    }

    public void Play()
    {
        State.Play(this);
    }

    public void Pause()
    {
        State.Pause(this);
    }

    public void Stop()
    {
        State.Stop(this);
    }
}