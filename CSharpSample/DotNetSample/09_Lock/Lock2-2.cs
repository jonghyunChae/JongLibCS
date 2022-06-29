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
            using (Semaphore semaphore = new Semaphore(2, 2, "Lock2-2"))
            {
                Console.WriteLine("Create Semaphore");

                semaphore.WaitOne(); //mut.WaitOne(Timeout.Infinite, false);
                Console.Write("Input >> ");
                Console.ReadLine();

                int v = semaphore.Release();
                Console.WriteLine("Release Semaphore : " + v);
            }

            Console.ReadLine();
            Console.WriteLine("Main End");

            Console.ReadKey();
        }
    }
}
