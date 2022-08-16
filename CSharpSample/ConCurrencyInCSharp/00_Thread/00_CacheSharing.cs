using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._00_Thread
{
    internal class _00_CacheSharing
    {
        // 여길 바꿔서 테스트. offset = 1, gap = 0 / offset = 16, gap = 0 / offset = 16, gap = 16
        const int offset = 1;
        const int gap = 0;
        public static int[] sharedData = new int[4 * offset + gap * offset];
        public static long DoFalseSharingTest(int threadsCount, int size =
            100_000_000)
        {
            Thread[] workers = new Thread[threadsCount];
            for (int i = 0; i < threadsCount; ++i)
            {
                workers[i] = new Thread(new ParameterizedThreadStart(idx =>
                {
                    int index = (int)idx + gap;
                    for (int j = 0; j < size; ++j)
                    {
                        sharedData[index * offset] = sharedData[index * offset] +
                                                     1;
                    }
                }));
            }
            for (int i = 0; i < threadsCount; ++i)
                workers[i].Start(i);
            for (int i = 0; i < threadsCount; ++i)
                workers[i].Join();
            return 0;
        }

        static void Main()
        {
            Stopwatch sw = Stopwatch.StartNew();
            DoFalseSharingTest(4);
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds + " ms");
        }
    }
}
