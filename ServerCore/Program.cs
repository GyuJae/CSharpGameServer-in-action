using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;


class Lock
{
    // bool <- 커널
    AutoResetEvent _available = new AutoResetEvent(true);    
    
    public void Acquire()
    {
        _available.WaitOne(); // 입장 시도
    }

    public void Release()
    {
        _available.Set(); // flag = true;
    }
}

class Program
{
    private static int _num = 0;
    private static Lock _lock = new Lock();

    static void Thread1()
    {
        for (int i = 0; i < 10000; i++)
        {
            _lock.Acquire();
            _num++;
            _lock.Release(); 
        }
    }

    static void Thread2()
    {
        for (int i = 0; i < 10000; i++)
        {
            _lock.Acquire();
            _num--;
            _lock.Release(); 
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

