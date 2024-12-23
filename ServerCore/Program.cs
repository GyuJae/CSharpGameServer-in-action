﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    private static int num = 0;
    private static object _obj = new object();
    
    static void Thread1()
    {
        for (int i = 0; i < 100000; i++)
        {
            // 상호배제 Mutual Exclusive
            Monitor.Enter(_obj);
            num++;
            Monitor.Exit(_obj);
        }
    }

    static void Thread2()
    {
        for (int i = 0; i < 100000; i++)
        {
            Monitor.Enter(_obj);
            num--;
            Monitor.Exit(_obj);
        } 
    }
    
    static void Main(string[] args)
    {
        Task t1 = new Task(Thread1);
        Task t2 = new Task(Thread2);
        
        t1.Start();
        t2.Start();
        
        Task.WaitAll(t1, t2);
        
        Console.WriteLine(num);
    }
}

