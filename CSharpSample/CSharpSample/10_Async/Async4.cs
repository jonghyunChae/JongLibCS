using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._10_Async
{
    class Async4
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
            Func();
        }

        static void Func()
        {
            DoFoo1();
            Console.WriteLine("Main Foo");

            DoFoo2();
            Console.WriteLine("Main Foo2");

            Console.ReadKey();
        }

        static async void DoFoo1()
        {
            Task task = Task.Run(new Action(Foo));
            // 하단부터는 쓰레드 풀이
            await task;
            Console.WriteLine("Foo1 Complete");
        }

        static async void DoFoo2()
        {
            Task<int> task2 = Task.Run(new Func<int>(Foo2));
            // 하단부터는 쓰레드 풀이
            int value = await task2;
            Console.WriteLine("Foo2 Complete. Value : " + value);
        }
    }
}
