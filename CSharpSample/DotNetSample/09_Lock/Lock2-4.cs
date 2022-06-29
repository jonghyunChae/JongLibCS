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
        static List<AutoResetEvent> events = new List<AutoResetEvent>();
        public static void Main()
        {
            Console.WriteLine("Main Start");

            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < 8; ++i)
            {
                threads.Add(new Thread(Work));
                events.Add(new AutoResetEvent(false));
            }

            for (int i = 0; i < 8; ++i)
            {
                threads[i].Start(i);
            }

            Console.Write("Input >> ");
            Console.ReadLine();
            events[7].Set();

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
            events[id].WaitOne();
            for (int i = 0; i < 1000000; ++i)
            {
                value += 2;
            }
            Console.WriteLine("Work Event Set : " + id);
            if (id > 0)
            {
                events[id - 1].Set();
            }
            Console.WriteLine("Work End : " + id);
        }
    }
}
