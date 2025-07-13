namespace PapersPlease;

public class WorkerHandler: BaseHandler<Person>
{
    public override void Handle(Person element)
    {
        if (element is not Worker worker)
        {
            Successor?.Handle(element);
            return;
        }

        var permit = worker.WorkPermit;
        if (permit is null)
        {
            element = worker.ToNormalPerson();
            Console.WriteLine($"- {element.Passport!.Name} (Worker) : no work permission, no work");
            Successor?.Handle(element);
            return;
        }
        if (!permit.Genuine)
        {
            element.EntryPermitted = false;
            Console.WriteLine($"- {element.Passport!.Name} (Worker) : fake work permission, arrested");
            return;
        }
        if (permit.Expired)
        {
            element = worker.ToNormalPerson();
            Console.WriteLine($"- {element.Passport!.Name} (Worker) : expired work permission, no work");
            Successor?.Handle(element);
            return;
        }
        
        Successor?.Handle(element);
    }
}