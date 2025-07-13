namespace JsonSerializer;

class Program
{
    public class Person
    {
        public required int Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required int Age { get; init; }
        public required string Type { get; init; }
        public required int Salary { get; init; }
    }
    
    static void Main(string[] args)
    {
        var path = @"C:\Users\VinderX\RiderProjects\TestSolution\JsonSerializer\Employees.csv";

        if (Path.GetExtension(path) != @".csv")
        {
            Console.WriteLine("The file format is incorrect.");
            return;
        }

        List<Person> people = [];
        
        //using var stream = new FileStream(path, FileMode.Open);
        using var reader = new StreamReader(path);
        reader.ReadLine(); // скипаем заголовки
        
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            var values = line.Split(',');
            people.Add(new Person()
            {
                Id = int.Parse(values[0]),
                FirstName = values[1],
                LastName = values[2],
                Age = int.Parse(values[3]),
                Type = values[4],
                Salary = int.Parse(values[5]),
            });
        }
        Console.WriteLine(people.Count);
        
        
    }
}