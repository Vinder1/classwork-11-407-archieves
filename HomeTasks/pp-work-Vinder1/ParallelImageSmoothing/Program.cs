using System.Diagnostics;

namespace ParallelImageSmoothing;

class Program
{
    private const int Height = 20, Width = 30;
    private const char Square = '\u25a0';

    private const int ThreadNum = 16; //perfomance boost only on big pictures.
    private const int D = 5; // more than 5 doesnt smooth anything (as tests show)
    private const int Iterations = 5; // too big values spoil the picture
    
    static void Main(string[] args)
    {
        var random = new Random();
        var picture = new bool[Height, Width];
        //Generate
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                picture[i, j] = (random.Next() & 1) == 0;
            }
        }
        //PreSmoothing (just making the picture look less random)
        //I do it while generating image, it is not meant to be considered as
        //a part of smoothing algorithm
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                if (ParallelSmoother.SumOfNeighborSquares(picture, i,j) > 5)
                    picture[i, j] = true;
                if (ParallelSmoother.SumOfNeighborSquares(picture, i,j) <= 2)
                    picture[i, j] = false;
            }
        }
        
        
        Console.WriteLine("\n[=-=-= Перед сглаживанием =-=-=]\n");
        Render(picture);
        
        //Smoothing
        var smoother = new ParallelSmoother(picture, ThreadNum, D, Iterations);
        var stopwatch = Stopwatch.StartNew();
        smoother.Smooth();
        stopwatch.Stop();
        
        Console.WriteLine("\n[=-=-= После сглаживания =-=-=]\n");
        Render(picture);

        Console.WriteLine(
            $"""
             
             [=-=-= Параметры =-=-=]
             - Размер изображения (в пикселях): {Height} x {Width}
             - Количество потоков: {ThreadNum}
             - D: {D}
             - Количество итераций: {Iterations}
             - Время на обработку: {(int)stopwatch.Elapsed.TotalMilliseconds} ms
             """);
    }

    private static void Render(bool[,] picture)
    {
        var height = picture.GetLength(0);
        var width = picture.GetLength(1);

        if (height * width > 100_000)
        {
            Console.WriteLine("Image is too big! Render cancelled");
            return;
        }
        
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                Console.Write(picture[i, j] ? Square : ' ');
            }
            Console.WriteLine();
        }
    }
}