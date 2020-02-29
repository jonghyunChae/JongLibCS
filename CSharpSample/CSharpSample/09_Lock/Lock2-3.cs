using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CSharpSample._9_Lock
{
    class Lock2
    {
        // AutoResetEvent (EventResetMode.AutoReset)
        // ManualResetEvent (EventResetMode.ManualReset)
        static EventWaitHandle evt = new EventWaitHandle(false, EventResetMode.AutoReset, "Lock2-3");
        public static void Main()
        {
            Console.WriteLine("Main Start");

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 8; ++i)
            {
                threads.Add(new Thread(Work));
            }

            for (int i = 0; i < 8; ++i)
            {
                threads[i].Start(i);
            }

            while (true)
            {
                Console.Write("Input >> ");
                if (Console.ReadLine() == "0")
                    break;

                evt.Set();
            }

            foreach (var th in threads)
            {
                th.Join();
            }

            Console.WriteLine("Main End");
            Console.ReadLine();

            Console.ReadKey();
        }

       
        static int value;
        public static void Work(object o)
        {
            int id = (int)o;
            Console.WriteLine("Work Start : " + id);
            evt.WaitOne();
            for (int i = 0; i < 1000000; ++i)
            {
                value += 2;
            }
            //Console.WriteLine("Work Event Set : " + id);
            //evt.Set();
            Console.WriteLine("Work End : " + id);
        }
    }
}
