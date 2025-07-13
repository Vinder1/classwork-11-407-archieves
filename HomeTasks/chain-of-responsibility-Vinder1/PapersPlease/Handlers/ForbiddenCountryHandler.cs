namespace PapersPlease;

public class ForbiddenCountryHandler: BaseHandler<Person>
{
    private static List<Country> forbiddenCountries = [];

    public static void SetForbiddenCountries(List<Country> countries)
    {
        forbiddenCountries = countries;
    }
    
    public override void Handle(Person element)
    {
        if (forbiddenCountries.Contains(element.Passport.Citizenship))
        {
            element.EntryPermitted = false;
            Console.WriteLine($"- {element.Passport.Name} : Citizenship of {element.Passport.Citizenship} is forbidden.");
            return;
        }
        Successor?.Handle(element);
    }
}