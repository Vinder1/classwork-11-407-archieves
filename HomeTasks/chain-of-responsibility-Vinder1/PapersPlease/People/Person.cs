namespace PapersPlease;

public class Person
{
    public Passport? Passport { get; init; }
    public EntryPermit? EntryPermit { get; init; }
    
    public bool? EntryPermitted { get; set; }
}