using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic
{
    internal class _07_TaskCompleteSource
    {
        static async Task WaitJob(Task task)
        {
            await Task.Yield();
            Console.WriteLine("[WaitJob] Start");
            try
            {
                await task;
            }
            catch (Exception e)
            {
                Console.WriteLine("[WaitJob] Exception Occur = " + e);
            }

            Console.WriteLine("[WaitJob] await End");
            Thread.Sleep(1000);
            Console.WriteLine("[WaitJob] End");
        }

        static async Task TaskCompleteTest()
        {
            tcs = new TaskCompletionSource();

            var t2 = Task.Run(() => WaitJob(tcs.Task));
            Console.WriteLine("[TaskCompleteTest] Start");
            Thread.Sleep(3000);
            Console.WriteLine("[TaskCompleteTest] Sleep End " + t2.IsCompleted);
            tcs.TrySetResult();
            Console.WriteLine("[TaskCompleteTest] TrySetResult");
            await t2;
            Console.WriteLine("[TaskCompleteTest] t2 End");
        }

        static async Task TimeoutTest()
        {
            tcs = new TaskCompletionSource();
            var cancel = new CancellationTokenSource(2000);
            using var canceled = cancel.Token.Register(() =>
            {
                Console.WriteLine("[TimeoutTest] Canceled!");
                if (!tcs.Task.IsCompleted)
                {
                    tcs.TrySetCanceled(cancel.Token);
                }
            }, useSynchronizationContext: false);

            var t2 = Task.Run(() => WaitJob(tcs.Task));
            Console.WriteLine("[TimeoutTest] Start");
            Thread.Sleep(5000);
            Console.WriteLine("[TimeoutTest] Sleep End " + t2.IsCompleted);
            await t2;
            Console.WriteLine("[TimeoutTest] t2 End");
        }

        static TaskCompletionSource tcs = null!;

        public class TestOptionAttribute : Attribute
        {
            public int Value;

            public TestOptionAttribute(int value)
            {
                Value = value;
            }
        }

        [TestOption(1)]
        class Test
        {
        }

        static async Task Main()
        {
            var fst = (TestOptionAttribute)typeof(Test).GetCustomAttributes(typeof(TestOptionAttribute), true).First();
            Console.WriteLine(fst.Value);


            await TaskCompleteTest();
            Console.WriteLine();
            await TimeoutTest();
        }
    }
}
