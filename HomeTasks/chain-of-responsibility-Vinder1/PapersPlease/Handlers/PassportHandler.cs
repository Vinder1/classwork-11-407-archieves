namespace PapersPlease;

public class PassportHandler: BaseHandler<Person>
{
    public override void Handle(Person element)
    {
        if (element.Passport is null)
        {
            element.EntryPermitted = false;
            Console.WriteLine($"- No passport, go back");
            return;
        }
        if (!element.Passport.Genuine)
        {
            element.EntryPermitted = false;
            Console.WriteLine($"- Fake passport, arrested");
            return;
        }
        Successor?.Handle(element);
    }
}