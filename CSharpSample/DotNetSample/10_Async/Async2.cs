using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._10_Async
{
    class Async2
    {
        public static void Foo()
        {
            for (int i = 0; i < 100; i++)
                Console.Write("Foo ");
        }

        public static int Foo2()
        {
            Console.WriteLine($"Foo ID : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"Foo end");

            return 1;
        }

        static void Main()
        {
            Task task = Task.Run(new Action(Foo));

            Thread.Sleep(1);

            Console.WriteLine(task.IsCompleted);

            task.Wait(); // 대기..

            Console.WriteLine(task.IsCompleted);

            Console.WriteLine();
            Console.WriteLine();


            Task<int> task2 = Task.Run(new Func<int>(Foo2));

            Console.WriteLine("Main");

            Console.WriteLine(task2.Result);

            Console.WriteLine("End");
        }
    }
}
