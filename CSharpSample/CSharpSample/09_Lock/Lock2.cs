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
        public static void Main()
        {
            Console.WriteLine("Main Start");

            Console.ReadLine();
            Mutex mut;
            bool created = false;
            if (Mutex.TryOpenExisting("abc", out mut) == false)
            {
                mut = new Mutex(false, "abc", out created);
            }
            Console.WriteLine("Create Mutex : " + created);

            mut.WaitOne(Timeout.Infinite, false);
            Console.WriteLine("Work Start");

            Thread.Sleep(10000);

            Console.WriteLine("Work End");

            Console.ReadLine();
            Console.WriteLine("Release Mutex");
            if (created)
            {
                mut.ReleaseMutex();
                mut.Dispose();
            }

            Console.ReadLine();
            Console.WriteLine("Main End");

            Console.ReadKey();
        }
    }
}
