namespace TypeWorkshop;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите: имя, возраст, курс, группа, есть ли питомец (True/False)");
        string name = Console.ReadLine();
        int age = int.Parse(Console.ReadLine());
        int course = int.Parse(Console.ReadLine());
        string group = Console.ReadLine();
        bool hasPet = bool.Parse(Console.ReadLine());

        Console.WriteLine("Ваша анкета:");
        Console.WriteLine("Имя          | " + name);
        Console.WriteLine("Возраст      | " + age);
        Console.WriteLine("Курс         | " + course);
        Console.WriteLine("Группа       | " + group);
        Console.WriteLine("Есть питомец | " + hasPet);
        Console.WriteLine("Есть питомец | " + hasPet);
        Console.WriteLine("Есть питомец | " + hasPet);
    }
}
