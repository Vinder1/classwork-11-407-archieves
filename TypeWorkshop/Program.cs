namespace TypeWorkshop;

class Program
{
    static void Main(string[] args)
    {
        // Console.WriteLine("Введите: имя, возраст, курс, группа, есть ли питомец (True/False)");
        // string name = Console.ReadLine();
        // int age = int.Parse(Console.ReadLine());
        // int course = int.Parse(Console.ReadLine());
        // string group = Console.ReadLine();
        // bool hasPet = bool.Parse(Console.ReadLine());

        // Console.WriteLine("Ваша анкета:");
        // Console.WriteLine("Имя          | " + name);
        // Console.WriteLine("Возраст      | " + age);
        // Console.WriteLine("Курс         | " + course);
        // Console.WriteLine("Группа       | " + group);
        // Console.WriteLine("Есть питомец | " + hasPet);
        // Console.WriteLine("Есть питомец | " + hasPet);
        // Console.WriteLine("Есть питомец | " + hasPet);

        if (args.Length < 3) {
            Console.WriteLine("Где аргументы?");
            return;
        }

        Console.WriteLine(CalculateOp(
            a: int.Parse(args[0]),
            b: int.Parse(args[1]),
            operand: args[2][0])
        );
    }

    static double CalculateOp(long a, long b, char operand)
    {
        // return operand == '+' ? a+b 
        // : operand == '-' ? a-b 
        // : operand == '*' ? a*b
        // : operand == '/' ? (double)a/b
        // : 0;

        switch (operand)
        {
            case '+':
                return a+b;
            case '-':
                return a-b;
            case '*':
                return a*b;
            case '/':
                return (double)a/b; 
            case '^':
                return Math.Pow(a, b);
            default:
                throw new NotSupportedException("Operrand not supported");
        }
    }
}
