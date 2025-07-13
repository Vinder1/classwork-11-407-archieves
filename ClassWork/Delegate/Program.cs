namespace Delegate;

class Program
{
    static void Main(string[] args)
    {
        var account = new Account(s => Console.WriteLine(s));
        account.Notify += s => Console.WriteLine(s+s);
        account.Notify += null;
        account.Invoke("a");

        int[] col = [1, 2, 3];
        var posNums  = col.Where(num => num % 2 == 0).ToArray();
    }
}