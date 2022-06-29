using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._7_GC
{
    class GC1
    {
        class MyClass
        {
            string name;
            public MyClass(string name)
            {
                this.name = name;
                Console.WriteLine($"{name} 생성");
            }
            ~MyClass()
            {
                Console.WriteLine($"{name} 소멸");
            }
        }

        static void Main()
        {
            Func("1");
            GC.Collect();
            //Thread.Sleep(1000);

            Func("2");
            GC.Collect();

            Console.ReadKey();
            var act = Capture("3");
            GC.Collect();

            Console.ReadKey();
            act = null;
            GC.Collect();

            Console.WriteLine("Main end");
        }

        static void Func(string name)
        {
            MyClass m = new MyClass(name);
        }

        static Action Capture(string name)
        {
            MyClass m = new MyClass(name);
            return () =>
            {
                Console.WriteLine(m);
            };
        }
    }
}
