using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._7_GC
{
    class GC2
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
                Console.WriteLine($"{name} 소멸 : Gen:{GC.GetGeneration(this)}");
            }
            public void Print()
            {
                Console.WriteLine($">>{name} : {GC.GetGeneration(this)}");
            }
        }

        static void Main()
        {
            MyClass m1 = new MyClass("1");
            MyClass m2 = new MyClass("2");
            m1.Print();
            m2.Print();
            m1 = null;
            GC.Collect();

            m2.Print();
            GC.Collect(0);

            m2.Print();
            GC.SuppressFinalize(m2);
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
