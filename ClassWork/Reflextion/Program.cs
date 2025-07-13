namespace Reflextion;

class Program
{
    static void Main(string[] args)
    {
        // var sportsman = new Sportsman
        //     {
        //         Name = "Егорчик",
        //         Age = 18,
        //         Rank = 43333,
        //     };
        // var student = Mapper.Map<Sportsman, Student>(sportsman);
        // Console.WriteLine($"{student.Name} {student.Age} {student.Course}");

        DependencyInjector.Register<IHuman, Sportsman>();
        Console.WriteLine(DependencyInjector.Resolve<IHuman>().GetType());
    }
}