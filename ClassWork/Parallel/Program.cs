namespace Parallel;

class Program
{
    static void Main(string[] args)
    {
        var t = 12.GetType();
        
        //GardenTask();
        //VectorTask();
    }

    

    
    //==============[Vectors]============
    
    private const int ThreadNums = 3;
    private const double K = 3.0;
    private static CountdownEvent countdownEvent = new CountdownEvent(ThreadNums);
    
    public static void VectorTask()
    {
        var vecs = new Vector3[1000];
        var rnd = new Random();
        for (var i = 0; i < vecs.Length; i++)
        {
            vecs[i] = new Vector3(rnd.Next() % 10, rnd.Next() % 10, rnd.Next() % 10);
            Console.Write($"({vecs[i].X}, {vecs[i].Y}, {vecs[i].Z}) ");
        }

        var threads = new Thread[ThreadNums];
        for (var i = 0; i < ThreadNums; i++)
        {
            var j = i;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                RingProduct(j, vecs);
                countdownEvent.Signal();
            });
            //ThreadPool.QueueUserWorkItem(CreateRingProduct(i, vecs));
        }
    
        countdownEvent.Wait();
        /*ThreadPool.QueueUserWorkItem(_ => RingProduct(0, vecs));
        ThreadPool.QueueUserWorkItem(_ => RingProduct(1, vecs));
        ThreadPool.QueueUserWorkItem(_ => RingProduct(2, vecs));*/
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        for (var i = 0; i < vecs.Length; i++)
        {
            //vecs[i] = new Vector3(rnd.Next(), rnd.Next(), rnd.Next());
            Console.Write($"({vecs[i].X}, {vecs[i].Y}, {vecs[i].Z}) ");
        }
    }
        
    public static void RingProduct(int start, Vector3[] vecs)
    {
        Console.WriteLine($"RingProduct({start})");
        for (var i = start; i < vecs.Length; i += ThreadNums)
        {
            vecs[i] = vecs[i].Product(K);
        }

        Console.WriteLine($"RingProduct({start}) end");
    }

    public record Vector3(double X, double Y, double Z)
    {
        public Vector3 Product(double k) => new Vector3(X * k, Y * k, Z * k);
    }

    //==========[Gardeners]===========

    public static void GardenTask()
    {
        var garden = new char[9, 11];
        var height = garden.GetLength(0);
        var width = garden.GetLength(1);

        for (var i = 0; i < height; i++)
        for (var j = 0; j < width; j++)
            garden[i, j] = '-';

        var t1 = new Thread(() => FirstGardener(garden));
        var t2 = new Thread(() => SecondGardener(garden));
        t1.Start();
        t2.Start();
        //FirstGardener(garden);
        //SecondGardener(garden);

        t1.Join();
        t2.Join();

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                Console.Write(garden[i, j]);
            }

            Console.WriteLine();
        }
    }

    public static void FirstGardener(char[,] garden)
    {
        var n = garden.GetLength(0);
        var m = garden.GetLength(1);

        var right = true;
        for (var y = 0; y < n; y++)
        {
            if (right)
            {
                for (var x = 0; x < m; x++)
                {
                    if (char.IsLetter(garden[y, x]))
                    {
                        m = x;
                        break;
                    }

                    garden[y, x] = 'a';
                    Thread.Sleep(1);
                }
            }
            else
            {
                for (var x = m - 1; x >= 0; x--)
                {
                    if (char.IsLetter(garden[y, x]))
                    {
                        m--;
                        continue;
                    }

                    garden[y, x] = 'a';
                    Thread.Sleep(1);
                }
            }

            right = !right;
        }
    }

    public static void SecondGardener(char[,] garden)
    {
        var n = garden.GetLength(0);
        var m = garden.GetLength(1);

        var up = true;
        var upperBound = 0;
        for (var x = m - 1; x >= 0; x--)
        {
            if (up)
            {
                for (var y = n - 1; y >= upperBound; y--)
                {
                    if (char.IsLetter(garden[y, x]))
                    {
                        upperBound = y;
                        break;
                    }

                    garden[y, x] = 'b';
                    Thread.Sleep(1);
                }
            }
            else
            {
                for (var y = upperBound; y < n; y++)
                {
                    if (char.IsLetter(garden[y, x]))
                    {
                        upperBound = y;
                        continue;
                    }

                    garden[y, x] = 'b';
                    Thread.Sleep(1);
                }
            }

            up = !up;
        }
    }
}