using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._9_Lock
{
    class Lock1
    {
        static int Value = 0;
        public static void Main()
        {
            Console.WriteLine("Main Start");

            DateTime dt = DateTime.Now;
            Thread th1 = new Thread(Func);
            Thread th2 = new Thread(Func);

            th1.Start(1);
            th2.Start(2);

            th1.Join();
            th2.Join();

            Console.WriteLine(Value);
            Console.WriteLine($"Main End : {(DateTime.Now - dt).TotalMilliseconds}");
            Console.ReadKey();
        }

        static object func_lock = new object();
        public static void Func(object obj)
        {
            for (int i = 0; i < 50000000; ++i)
            {
                Monitor.Enter(func_lock);
                Value += 2;
                Monitor.Exit(func_lock);
            }

            Console.WriteLine(obj + " End");
        }
    }
}
