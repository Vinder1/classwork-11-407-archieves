namespace PapersPlease;

class Program
{
    static void Main(string[] args)
    {
        // invalid work permit
        var gosha = new Worker
        {
            Passport = new Passport
            {
                Citizenship = Country.Impor,
                Genuine = true,
                Name = "Гоша"
            },
            EntryPermit = new EntryPermit
            {
                Expired = false,
                Genuine = true,
            },
            WorkPermit = new WorkPermit
            {
                Expired = true,
                Genuine = true,
            }
        };
        
        //everything fine
        var igor = new Person
        {
            Passport = new Passport
            {
                Citizenship = Country.Orbistan,
                Genuine = true,
                Name = "Игорь"
            },
            EntryPermit = new EntryPermit
            {
                Expired = false,
                Genuine = true,
            }
        };
        
        //Arstotzkian
        var dasha = new Person
        {
            Passport = new Passport
            {
                Citizenship = Country.Arstotzka,
                Genuine = true,
                Name = "Даша"
            }
        };
        
        //oleg. Punish him for being oleg.
        var oleg = new Person
        {
            Passport = new Passport
            {
                Citizenship = Country.Antegria,
                Genuine = true,
                Name = "Олег"
            },
            EntryPermit = new EntryPermit
            {
                Expired = false,
                Genuine = true,
            }
        };
        
        //banned for being kolechian
        var grisha = new Person
        {
            Passport = new Passport
            {
                Citizenship = Country.Kolechia,
                Genuine = true,
                Name = "Гриша"
            },
            EntryPermit = new EntryPermit
            {
                Expired = false,
                Genuine = true,
            }
        };

        var handlers = new HandlersSequence<Person>();
        handlers.AddHandler(new PassportHandler());
        handlers.AddHandler(new ForbiddenCountryHandler());
        handlers.AddHandler(new CitizenHandler());
        handlers.AddHandler(new WorkerHandler());
        handlers.AddHandler(new CriminalHandler());
        handlers.AddHandler(new FinalHandler());
        
        CriminalHandler.SetCriminals(["Олег"]);
        ForbiddenCountryHandler.SetForbiddenCountries([Country.Kolechia]);
                
        Console.WriteLine("Обработка Гоши");
        handlers.Handle(gosha);
        Console.WriteLine("Обработка Игоря");
        handlers.Handle(igor);
        Console.WriteLine("Обработка Даши");
        handlers.Handle(dasha);
        Console.WriteLine("Обработка Олега");
        handlers.Handle(oleg);
        Console.WriteLine("Обработка Гриши");
        handlers.Handle(grisha);
    }
}