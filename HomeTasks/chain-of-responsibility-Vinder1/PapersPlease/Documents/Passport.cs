namespace PapersPlease;

public class Passport : IDocument
{
    public required bool Genuine { get; init; }
    public required string Name { get; init; }
    public required Country Citizenship { get; init; }
}