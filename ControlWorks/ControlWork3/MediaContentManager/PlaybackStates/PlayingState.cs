namespace MediaContentManager;

public class PlayingState : IPlaybackState
{
    public void Play(MediaPlayer player)
    {
        
    }

    public void Pause(MediaPlayer player)
    {
        player.ChangeState(new PausedState());
    }

    public void Stop(MediaPlayer player)
    {
        player.ChangeState(new StoppedState());
    }
}