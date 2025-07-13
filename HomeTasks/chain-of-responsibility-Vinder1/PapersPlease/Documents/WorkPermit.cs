namespace PapersPlease;

public class WorkPermit : IPermit
{
    public required bool Genuine { get; init; }
    public required bool Expired { get; init; }
}