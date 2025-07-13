namespace PapersPlease;

public class CriminalHandler: BaseHandler<Person>
{
    private static List<string> criminalNames = [];

    public static void SetCriminals(List<string> criminals)
    {
        criminalNames = criminals;
    }
    
    public override void Handle(Person element)
    {
        if (criminalNames.Contains(element.Passport.Name))
        {
            element.EntryPermitted = false;
            Console.WriteLine($"- {element.Passport.Name} : Criminal! Arrested.");
            return;
        }
        Successor?.Handle(element);
    }
}