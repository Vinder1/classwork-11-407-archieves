using System.Diagnostics;
namespace loops;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(RomanToInt("MCMXCIV")); //1994..?
    }


    /// Home Task 1
    public static int RomanToInt(string s) {
        int answer = 0;
        int prev = 0;
        for (int i = s.Length - 1; i >= 0; i--)
        {
            var nextSymbolValue = RomanCharToInt(s[i]);
            if (nextSymbolValue < prev)
                answer -= nextSymbolValue;
            else
                answer += nextSymbolValue;
            prev = nextSymbolValue;
        }    
        return answer;
    }

    private static int RomanCharToInt(char from)
    {
        switch (from)
        {
            case 'I':
                return 1;
            case 'V':
                return 5;
            case 'X':
                return 10;
            case 'L':
                return 50;
            case 'C':
                return 100;
            case 'D':
                return 500;
            case 'M':
                return 1000;
            default:
                return 0;
        } 
    }


    ///Home Task 2
    public static void Rotate(int[][] matrix) {
        int n = matrix.Length;
        for (int y = 0; y < n; y++)
        {
            for (int x = y; x < n; x++)
            {
                (matrix[y][x], matrix[x][y]) = (matrix[x][y], matrix[y][x]);
            }
        }
        for (int y = 0; y < n; y++)
        {
            for (int x = 0; x < n / 2; x++)
            {
                (matrix[y][x], matrix[y][n-1-x]) = (matrix[y][n-1-x], matrix[y][x]);
            }
        }
    }


    ///Home Task 3 (O(n))
    public static void MoveZeroes(int[] nums) {
        //in fact, the task is to 'compress' an array: to rewrite it, skipping all the zeros;
        //newArrayIndex <= i AND everything behind i will be unused =>
        // => behind i there will be always enough space for our new array =>
        // => lets write everything right there
        int newArrayIndex = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == 0)
                continue;
            if (newArrayIndex == i)
            {
                newArrayIndex++;
                continue;
            }
            nums[newArrayIndex++] = nums[i];
            nums[i] = 0;
        }
    }


    ///Home Task 3 (O(n^2))
    public static void MoveZeroesN2(int[] nums) {
        int n = nums.Length;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n-1-i; j++)
            {
                if (nums[j] == 0)
                {
                    (nums[j], nums[j+1]) = (nums[j+1], nums[j]);
                }
            }
        }
    }



    /// Classwork


    // static void Main(string[] args)
    // {

    //     var stopWatch = new Stopwatch();
    //     stopWatch.Start();
    //     var n = int.Parse(args[0]);
    //     int[] ans = new int[n];
    //     // _ = FibonacciRecursion(n, ans);
    //     // for (var i = 0; i < n; i++)
    //     // {
    //     //     Console.Write($"{ans[i]} ");
    //     // }
    //     FibonacciLoop(n);

    //     stopWatch.Stop();
    //     Console.WriteLine($"\nRunTime {stopWatch.ElapsedMilliseconds} ms | {stopWatch.ElapsedTicks} ticks");
    //     Console.WriteLine(CheckPalindrome(10));
    //     Console.WriteLine(CheckWords("hello", "any", "people"));
    //     Console.WriteLine(ReverseWords("One Two Three Four Five"));
    //     Console.WriteLine(ConvertAgeInFullName(1));
    //     Console.WriteLine(ConvertAgeInFullName(2));
    //     Console.WriteLine(ConvertAgeInFullName(6));
    //     Console.WriteLine(ConvertAgeInFullName(15));
    //     Console.WriteLine(ConvertAgeInFullName(43));
    // }

    static string ReverseWords(string text)
    {
        var words = text.Split();
        if (words.Length < 2)
            throw new ArgumentException("Incorrect input: less than 2 words in text");
        (words[0], words[^1]) = (words[^1], words[0]);
        return string.Join(" ", words);
    }

    static string ConvertAgeInFullName(int n)
    {
        if (n < 1 || n > 100)
            throw new ArgumentException("Incorrect input: age is <1 or >100");
        if (11 <= n && n <= 19 || n % 10 == 0)
            return "лет";
        if (n % 10 == 1)
            return "год";
        if (n % 10 < 5)
            return "года";
        return "лет";
    }

    static bool CheckPalindrome(int n)
    {
        if (n > 9999 || n <= 0)
            throw new ArgumentException("n out of bounds");

        var digit1 = n / 1000;
        var digit2 = n / 100 % 10;
        var digit3 = n % 100 / 10;
        var digit4 = n % 10;
        return digit1 == digit4 && digit2 == digit3;
    }

    static string CheckWords(string a, string b, string c)
    {
        return string.Concat(
            CheckWord(a,b,c),
            CheckWord(b,a,c),
            CheckWord(c,b,a)
        );
    }

    static string CheckWord(string iterable, string a, string b)
    {
        var answer = "";
        foreach (var c in iterable)
        {
            if (!a.Contains(c) && !b.Contains(c))
                answer += $"{c}";
        }
        return answer;
    }


    static int FibonacciRecursion(int n, int[] ans)
    {
        if (n == 1 || n == 2)
        {
            ans[n-1] = 1;
            return 1;
        }

        var result = FibonacciRecursion(n-1, ans) + FibonacciRecursion(n-2, ans);
        ans[n-1] = result;
        return result;
    }

    static void FibonacciLoop(int n)
    {
        ulong prev = 0;
        ulong current = 1;
        Console.Write("0 ");


        for (var i = 1; i < n; i++)
        {
            Console.Write($"{current} ");
            current = current + prev;
            prev = current - prev;
        }
    }
}
