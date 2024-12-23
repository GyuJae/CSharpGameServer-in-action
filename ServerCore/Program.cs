using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    static void Main(string[] args)
    {
        int [,] arr = new int[10000, 10000];

        {
            long now = DateTime.Now.Ticks;
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    arr[y, x] = 1;
                }
            }
            long end = DateTime.Now.Ticks;
            Console.WriteLine($"(y, x) 걸린 시간: {now - end}");
        }
        
        {
            long now = DateTime.Now.Ticks;
            for (int y = 0; y < arr.GetLength(0); y++)
            {
                for (int x = 0; x < arr.GetLength(1); x++)
                {
                    arr[x, y] = 1;
                }
            }
            long end = DateTime.Now.Ticks;
            Console.WriteLine($"(x, y) 걸린 시간: {now - end}");
        }
    }
}

