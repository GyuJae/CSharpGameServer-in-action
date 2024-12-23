﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{
    static void MainThread(object state)
    {
        for(var i = 0; i < 5; i++)
        {
            Console.WriteLine("Main Thread");
        }
        
    }
    
    static void Main(string[] args)
    {
        ThreadPool.SetMinThreads(1, 1);
        ThreadPool.SetMaxThreads(5, 5);

        for (var i = 0; i < 4; i++)
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                while (true)
                {
                }
            });
        }
        
        ThreadPool.QueueUserWorkItem(MainThread);
        
        while(true) {}
    }
}

