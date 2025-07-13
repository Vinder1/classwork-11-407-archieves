namespace MediaContentManager;

public class PausedState : IPlaybackState
{
    public void Play(MediaPlayer player)
    {
        player.ChangeState(new PlayingState());
    }

    public void Pause(MediaPlayer player)
    {
        
    }

    public void Stop(MediaPlayer player)
    {
        player.ChangeState(new StoppedState());
    }
}