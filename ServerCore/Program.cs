using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    private static volatile bool _stop = false;

    static void ThreadMain()
    {
        Console.WriteLine("Thread Start!");
        
        while (_stop == false)
        {
            // 누군가의 신호를 줄때까지 대기한다.
        }
        Console.WriteLine("Thread End!");
    }

    
    static void Main(string[] args)
    {
        Task t = new Task(ThreadMain);
        t.Start();
        
        Thread.Sleep(1000);

        _stop = true;

        Console.WriteLine("Stop 호출");
        Console.WriteLine("종료 대기중");
        
        t.Wait();
        
        Console.WriteLine("종료 성공");
    }
}

