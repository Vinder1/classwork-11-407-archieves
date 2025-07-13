namespace PapersPlease;

public class Worker : Person
{
    public WorkPermit? WorkPermit { get; init; }

    public Person ToNormalPerson()
    {
        return new Person
        {
            EntryPermit = this.EntryPermit,
            Passport = this.Passport
        };
    }
}