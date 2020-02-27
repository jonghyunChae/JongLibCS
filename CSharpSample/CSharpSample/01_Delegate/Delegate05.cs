using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._01_Delegate
{
    class Delegate04
    {
        class Test
        {
            public int v;
            ~Test()
            {
                Console.WriteLine("~Test");
            }
        }

        public static void Main()
        {
            func();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("1");

            var act = func();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("2");

            act(1);
            act = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine("Main end");
        }

        static Action<int> func()
        {
            Test t = new Test();
            t.v = 2;

            //Action<int> act2 = (int v1) => {};
            //Action<int> act2 = (v1) => {};
            return v1 => { Console.WriteLine($"func:{t.v + v1}"); };
        }
    }
}
