using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._10_Async
{
    class Async2
    {
        public static void Foo()
        {
            Console.WriteLine($"Foo1 ID : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"Foo1 end");
        }

        public static int Foo2()
        {
            Console.WriteLine($"Foo2 ID : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine($"Foo2 end");

            return 1;
        }

        static void Main()
        {
            Task task = Task.Run(new Action(Foo));
            TaskAwaiter awatier = task.GetAwaiter();
            awatier.OnCompleted(() =>
            {
                Console.WriteLine("Foo1 Complete");
            });

            Console.WriteLine("Main Foo");

            Task<int> task2 = Task.Run(new Func<int>(Foo2));
            TaskAwaiter<int> awatier2 = task2.GetAwaiter();
            awatier2.OnCompleted(() =>
            {
                Console.WriteLine("Foo2 Complete. Value : " + task2.Result);
            });
            Console.WriteLine("Main Foo2");

            Console.ReadKey();
        }
    }
}
