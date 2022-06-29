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
        public static void Main()
        {
            Thread th1 = new Thread(Func);
            Thread th2 = new Thread(Func);

            th1.Start(1);
            th2.Start(2);
            for (int i = 0; i < 10; ++i)
            {
                Console.WriteLine("0");
                Thread.Sleep(10);
                
            }
            th1.Abort();
            th2.Abort();
        }

        public static void Func(object obj)
        {
            while(true)
            {
                Console.WriteLine((int)obj);
                Thread.Sleep(10);
            }
        }
    }
}
