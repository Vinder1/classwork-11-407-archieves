namespace ControlWork2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("КР 2 Вариант 2");

        RepairWork[] works =
        [
            new ApartmentRepair(
                location: "ул Барбосскина, д 52",
                deadline: new DateTime(2026, 12, 10),
                floorNumber: 4
                ),
            
            new HomeRepair(
                location: "ул Бобриной печёнки, д 229",
                deadline: new DateTime(2077, 11, 08),
                hasGarden: true
            ),
            
            new OfficeRepair(
                location: "ул Волл-стрит, д 23456543",
                deadline: new DateTime(2025, 5, 10),
                numberOfWorkstations: 13
            ),
        ];
        
        Console.WriteLine("Описания объектов:");
        foreach (var work in works)
        {
            Console.WriteLine(work.GetDetails());
            Console.WriteLine();
        }
        
        Console.WriteLine($"Цена за все: {works.CalculateTotalCost()}");
        
        var manager = new RepairManager();
        Console.WriteLine("Приглашен манагер");
        manager.Schedule(DateTime.Today);
        
        Console.WriteLine("Добавлена работа");
        manager.AddWork(works[0]);
        manager.Schedule(DateTime.Today);

        try
        {
            var invalidApartmentRepair = new ApartmentRepair(
                location: "aaaa",
                deadline: new DateTime(2026, 12, 10),
                floorNumber: -10
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        
        Console.WriteLine(manager.GetWorksByDeadline(new DateTime(2026, 12, 10)).Length);
        Console.WriteLine(manager.GetWorksByDeadline(new DateTime(2077, 11, 8)).Length);
        Console.WriteLine(works[1].StartDate);
    }
}