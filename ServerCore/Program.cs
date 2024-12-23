using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore;

class Program
{

    class SessionManager
    {
        static object _lock = new object();

        public static void Test()
        {
            lock (_lock)
            {
               UserManager.TestUser(); 
            }
        }

       public static void TestSession()
        {
            lock (_lock)
            {
                
            }
        }
    }

    class UserManager
    {
        static object _lock = new object();

        public static void TestUser()
        {
            lock (_lock)
            {
                
            }
        }

        public static void Test()
        {
            lock (_lock)
            {
                SessionManager.TestSession();
            }
        }
        
    }
    
    private static int num = 0;
    
    static void Thread1()
    {
        for (int i = 0; i < 100000; i++)
        {
            UserManager.Test();
        }
    }

    static void Thread2()
    {
        for (int i = 0; i < 100000; i++)
        {
            SessionManager.Test();
        }
    }
    
    static void Main(string[] args)
    {
        Task t1 = new Task(Thread1);
        Task t2 = new Task(Thread2);
        
        t1.Start();
        t2.Start();
        
        Task.WaitAll(t1, t2);
        
        Console.WriteLine(1);
    }
}

