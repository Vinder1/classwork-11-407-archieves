namespace PapersPlease;

public class FinalHandler: BaseHandler<Person>
{
    public override void Handle(Person element)
    {
        element.EntryPermitted = true;
        Console.WriteLine($"- {element.Passport.Name} : Wellcome to Arstotzka!");
    }
}