using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;


class SpinLock
{
    volatile int _locked = 0;
        
    public void Acquire()
    {
        // // 동시에 들어 갈 수 있음;
        // while (_locked)
        // {
        //     // 잠김이 풀리기를 기다린다.
        // }
        // _locked = true;
        while (true)
        {
            // version. 1
            // int original = Interlocked.Exchange(ref _locked, 1);
            // if (original == 0) break;
            
            // CAS Compare-And-Swap
            int expected = 0;
            int desired = 1;
            if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected) break;
            
            // 쉬다 오기
            Thread.Sleep(1); // 무조건 쉬기
            Thread.Sleep(0); // 조건부 양보 -> 나보다 우선순위가 낮은 애들한테는 양보불가 => 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인한테
            Thread.Yield(); // 관대한 양 -> 관대하게 양보할테니, 지금 실행이 가능한 쓰레드가 있으면 실행하세요 -> 실행 가능한 애가 없으면 남은 시간 소진
        }
    }

    public void Release()
    {
        _locked = 0;
    }
}

class Program
{
    private static int _num = 0;
    private static SpinLock _lock = new SpinLock();

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

