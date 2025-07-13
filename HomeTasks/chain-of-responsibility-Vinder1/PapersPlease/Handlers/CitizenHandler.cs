namespace PapersPlease;

public class CitizenHandler: BaseHandler<Person>
{
    public override void Handle(Person element)
    {

        if (element.Passport.Citizenship == Country.Arstotzka)
        {
            // Not foreign
            Console.WriteLine($"- {element.Passport!.Name} : Glory to Arstotzka!");
        }
        else
        {
            var permission = element.EntryPermit;
            if (permission is null)
            {
                element.EntryPermitted = false;
                Console.WriteLine($"- {element.Passport!.Name} : no entry permission, go back");
                return;
            }
            else if (permission.Expired)
            {
                element.EntryPermitted = false;
                Console.WriteLine($"- {element.Passport!.Name} : expired entry permission, go back");
                return;
            }
            else if (!permission.Genuine)
            {
                element.EntryPermitted = false;
                Console.WriteLine($"- {element.Passport!.Name} : fake entry permission, arrested");
                return;
            }
        }
        Successor?.Handle(element);
    }
}