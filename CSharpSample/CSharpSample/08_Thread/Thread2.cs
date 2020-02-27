using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._8_Thread
{
    class Thread1
    {
        static int Value = 0;
        public static void Main()
        {
            Thread th1 = new Thread(Func);
            Thread th2 = new Thread(Func);

            th1.Start(1);
            th2.Start(2);

            th1.Join();
            th2.Join();

            Console.WriteLine(Value);
            Console.WriteLine("Main End");
        }

        public static void Func(object obj)
        {
            for (int i = 0; i < 500000000; ++i)
            {
                Value += 2;
            }

            Console.WriteLine(obj + " End");
        }
    }
}
