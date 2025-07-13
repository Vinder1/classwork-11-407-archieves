namespace PapersPlease;

public class EntryPermit : IPermit
{
    public required bool Genuine { get; init; }
    public required bool Expired { get; init; }
}