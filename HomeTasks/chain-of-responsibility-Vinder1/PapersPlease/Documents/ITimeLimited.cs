namespace PapersPlease;

public interface ITimeLimited
{
    public bool Expired { get; init; }
}