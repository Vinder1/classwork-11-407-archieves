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

    ///Home Task 1
    public string AddBinary(string a, string b)
    {
        if (a.Length < b.Length)
            (a, b) = (b, a);
        bool[] reformatedA = new bool[a.Length + 1];
        for (int i = 0; i < a.Length; i++)
            reformatedA[i] = a[^(i+1)] == '1';

        bool toNext = false;
        for (int i = 0; i < b.Length; i++)
        {
            var next = b[^(i+1)] == '1';
            var newDigit = (next?1:0) + (toNext?1:0) + (reformatedA[i]?1:0);
            toNext = (newDigit > 1);
            reformatedA[i] = newDigit % 2 == 1;
        }
        var j = b.Length;
        while (toNext && j < reformatedA.Length)
        {
            var newDigit = (toNext?1:0) + (reformatedA[j]?1:0);
            toNext = (newDigit > 1);
            reformatedA[j] = newDigit % 2 == 1;
            j++;
        }

        var startIndex = reformatedA[^1] ? 0 : 1; //so we will skip first 0 if it exists
        char[] newString = new char[reformatedA.Length - startIndex];
        for (int i = startIndex; i < reformatedA.Length; i++)
            newString[i-startIndex] = reformatedA[^(i+1)] ? '1' : '0';
        return new(newString);
    }


    ///Home Task 2
    public static bool IsPowerOfTwo(int n)
    {
        return n > 0 && (n & (n-1)) == 0;
    }

    ///Home Task 3
    public static int AddDigits(int num)
    {
        //if p(n) is sum of digits, then n % 9 = p(n) % 9
        if (num == 0)
            return 0;
        if (num % 9 == 0)
            return 9;
        return num % 9;
    }

    ///Home Task 4
    public static bool IsUgly(int n)
    {
        if (n == 0)
            return false;
        var n2 = n;
        while (n2 % 2 == 0)
            n2 /= 2;
        while (n2 % 3 == 0)
            n2 /= 3;
        while (n2 % 5 == 0)
            n2 /= 5;
        return n2 == 1;
    }
}
