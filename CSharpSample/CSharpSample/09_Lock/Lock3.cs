using System;
using System.Threading;

namespace CSharpSample._9_Lock
{
    class Lock3
    {
        static long Value = 0;
        public static void Main()
        {
            Console.WriteLine("Main Start");

            DateTime dt = DateTime.Now;
            Thread[] thlist = new Thread[8];
            for (int i = 0; i < 8; ++i)
            {
                if (i % 2 == 0)
                {
                    thlist[i] = new Thread(Func);
                }
                else
                {
                    thlist[i] = new Thread(Func2);
                }
            }

            for (int i = 0; i < 8; ++i)
            {
                thlist[i].Start(i);
            }

            foreach (var th in thlist)
            {
                th.Join();
            }

            Console.WriteLine(Value);
            Console.WriteLine($"Main End : {(DateTime.Now - dt).TotalMilliseconds}");
            Console.ReadKey();
        }


        static object func_lock1 = new object();
        static object func_lock2 = new object();

        public static void Func(object obj)
        {
            for (int i = 0; i < 50000000; ++i)
            {
                Monitor.Enter(func_lock1);
                Monitor.Enter(func_lock2);
                Value += 2;
                Monitor.Exit(func_lock1);
                Monitor.Exit(func_lock2);
            }


            Console.WriteLine(obj + " End");
        }

        public static void Func2(object obj)
        {
            for (int i = 0; i < 50000000; ++i)
            {
                Monitor.Enter(func_lock2);
                Monitor.Enter(func_lock1);
                Value += 2;
                Monitor.Exit(func_lock2);
                Monitor.Exit(func_lock1);
            }


            Console.WriteLine(obj + " End");
        }
    }
}
