using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._10_Async
{
    class Async1
    {
        static void Main()
        {
            int value = 0;
            WaitCallback callback = state =>
            {
                for (int i = 0; i < 1000000; ++i)
                {
                    value += 2;
                }
            };
            ThreadPool.QueueUserWorkItem(callback);
            ThreadPool.QueueUserWorkItem(callback);
            ThreadPool.QueueUserWorkItem(callback);
            ThreadPool.QueueUserWorkItem(callback);
            ThreadPool.QueueUserWorkItem(callback);
            ThreadPool.QueueUserWorkItem(callback);

            Thread.Sleep(10000);
            Console.WriteLine(value);
        }
    }
}
