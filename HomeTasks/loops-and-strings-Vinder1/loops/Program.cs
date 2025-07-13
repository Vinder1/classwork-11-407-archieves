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
            {
                answer -= nextSymbolValue;
            } 
            else
            {
                answer += nextSymbolValue;
            }
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
                throw new ArgumentException("Incorrect input");
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
            {
                continue;
            }
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
}
