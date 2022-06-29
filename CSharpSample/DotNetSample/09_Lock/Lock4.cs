using System;
using System.Threading;

namespace CSharpSample._9_Lock
{
    class Lock4
    {
        static long Value = 0;
        public static void Main()
        {
            Console.WriteLine("Main Start");

            DateTime dt = DateTime.Now;
            Thread[] thlist = new Thread[8];
            for (int i = 0; i < 8; ++i)
            {
                thlist[i] = new Thread(Func);
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
        public static void Func(object obj)
        {
            for (int i = 0; i < 50000000; ++i)
            {
                AddValue1();
            }

            Console.WriteLine(obj + " End");
        }

        public static void AddValue1()
        {
            Monitor.Enter(func_lock1);
            Value += 2;
            if (Value >= 1000)
            {
                return;
            }

            Monitor.Exit(func_lock1);
        }

        public static void AddValue2()
        {
            lock (func_lock1)
            {
                Value += 2;
                if (Value >= 1000)
                {
                    return;
                }
            }
        }
    }
}
