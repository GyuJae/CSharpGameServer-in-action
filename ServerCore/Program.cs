using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    private static int _num = 0;
    private static Mutex _lock = new Mutex();

    static void Thread1()
    {
        for (int i = 0; i < 10000; i++)
        {
            _lock.WaitOne();
            _num++;
            _lock.ReleaseMutex(); 
        }
    }

    static void Thread2()
    {
        for (int i = 0; i < 10000; i++)
        {
            _lock.WaitOne();
            _num--;
            _lock.ReleaseMutex(); 
        }
    }
    
    static void Main(string[] args)
    {
        var t1 = new Task(Thread1);
        var t2 = new Task(Thread2);
        
        t1.Start();
        t2.Start();
        
        Task.WaitAll(t1, t2);
        
        Console.WriteLine(_num);
    }
}

