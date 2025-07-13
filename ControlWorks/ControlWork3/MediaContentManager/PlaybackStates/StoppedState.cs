namespace MediaContentManager;

public class StoppedState : IPlaybackState
{
    public void Play(MediaPlayer player)
    {
        player.ChangeState(new PlayingState());
    }

    public void Pause(MediaPlayer player)
    {
        player.ChangeState(new PausedState());
    }

    public void Stop(MediaPlayer player)
    {
        
    }
}