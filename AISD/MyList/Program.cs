namespace MyList;

class Program
{
    static void Main(string[] args)
    {
        MyList<int> list = [2];
        list.Add(1);
        list.Add(4);
        list.Add(44);
        list.Add(3);
        list.Add(15);
        list.Remove(3);
        Console.WriteLine(string.Join(", ", list));
    }
}