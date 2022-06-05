using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic
{
    internal class _02_Progress
    {
        /*
          /// <summary>Reports a progress change.</summary>
          /// <param name="value">The value of the updated progress.</param>
            protected virtual void OnReport(T value)
            {
                // If there's no handler, don't bother going through the sync context.
                // Inside the callback, we'll need to check again, in case
                // an event handler is removed between now and then.
                Action<T>? handler = _handler;
                EventHandler<T>? changedEvent = ProgressChanged;
                if (handler != null || changedEvent != null)
                {
                    // Post the processing to the sync context.
                    // (If T is a value type, it will get boxed here.)
                    _synchronizationContext.Post(_invokeHandlers, value);
                }
            }
         */

        static async Task Main()
        {
            // 순서 보장 되지 않는다.
            var progress = new Progress<int>();
            progress.ProgressChanged += (object sender, int value) =>
            {
                Console.WriteLine("Progress " + value);
            };
            await DoAsync(progress);
            Thread.Sleep(1000);

            // Mutable한 참조 객체로 넘길 경우 문제
            var progress2 = new Progress<Wrapper>();
            progress2.ProgressChanged += (object sender, Wrapper value) =>
            {
                Console.WriteLine("Wrapper Progress " + value.Value);
            };
            await DoAsync2(progress2);

            Thread.Sleep(1000);
        }

        static async Task DoAsync(IProgress<int> progress)
        {
            for (int i = 0; i < 100; ++i)
            {
                progress.Report(i);
            }
        }

        class Wrapper
        {
            public int Value { get; set; }
        }

        static async Task DoAsync2(IProgress<Wrapper> progress)
        {
            Wrapper wr = new Wrapper();
            for (int i = 0; i < 100; ++i)
            {
                wr.Value = i;
                progress.Report(wr);
            }
        }

    }
}
