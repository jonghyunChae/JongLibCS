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

            bool created = false;
            //if (Mutex.TryOpenExisting("abc", out mut) == false)
            //{
            //    mut = new Mutex(false, "abc", out created);
            //}

            using (Mutex mut = new Mutex(false, "abc", out created))
            {
                Console.WriteLine("Create Mutex : " + created);

                // 소유자가 자원을 제대로 Release 하지 않으고 종료되면 AbandonedMutexException를 던진다.
                mut.WaitOne(); //mut.WaitOne(Timeout.Infinite, false);

                Console.Write("Input >> ");
                if (Console.ReadLine() == "1")
                    return;

                Console.WriteLine("Release Mutex");
                // wait 수 당 Release 한 번씩
                mut.ReleaseMutex();
            }

            Console.ReadLine();
            Console.WriteLine("Main End");

            Console.ReadKey();
        }
    }
}
