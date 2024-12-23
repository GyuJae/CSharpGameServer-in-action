using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    // 메모리 배리어
    // A) 코드 재배치 억제
    // B) 가시성
    
    // 1) Full Memory Barrier (ASM MFENCE, C# Thread.MemoryBarrier): Store/ Load 둘다 막음
    // 2) Store Memory Barrier (ASM SFENCE C# Thread. : Store
    // 3) Load Memory Barrier  ASM LFENCE : Load
    
    private static int x = 0;
    private static int y = 0;
    private static int r1 = 0;
    private static int r2 = 0;

    static void Thread_1()
    {
        y = 1; // Store Y
    
        Thread.MemoryBarrier();
        
        r1 = x; // Load X
    }

    static void Thread_2()
    {
        x = 1; // Store X
        
        Thread.MemoryBarrier();
        
        r2 = y; // Load Y
    }
    
    static void Main(string[] args)
    {
        int cnt = 0;
        while (true)
        {
            cnt++;
            x = y = r1 = r2 = 0;
            
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);
        
            t1.Start();
            t2.Start();
        
            Task.WaitAll(t1, t2);

            if (r1 == 0 && r2 == 0)
            {
                break;
            }
        }
        
        Console.WriteLine($"{cnt}번에 빠져나옴");
    }
}

