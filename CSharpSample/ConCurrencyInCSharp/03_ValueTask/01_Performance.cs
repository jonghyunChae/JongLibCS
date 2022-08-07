using System.Diagnostics;

class Program
{
    // https://www.sysnet.pe.kr/2/0/13115
    static async Task<int> Main(string[] args)
    {
        Func<int, string, Func<int, int, Task>, int, Task> action = async (loopCount, title, work, delay) =>
        {
            Stopwatch st = new Stopwatch();
            st.Start();

            await work(loopCount, delay);

            st.Stop();

            Console.WriteLine(title + " : " + st.ElapsedMilliseconds);
        };

        await action(1, "ValueAsync for JIT", ValueAsync, 0);
        await action(1, "RefAsync for JIT", RefAsync, 0);

        Console.WriteLine();

        WriteCollectionCount();
        await action(1_000_000, "ValueAsync", ValueAsync, 0);
        WriteCollectionCount();
        await action(1_000_000, "RefAsync", RefAsync, 0);
        WriteCollectionCount();

        return 0;
    }

    static void WriteCollectionCount()
    {
        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);

        Console.WriteLine($"{gen0 + gen1 + gen2}, ({gen0}), ({gen1}), ({gen2})");
    }

    private static async Task ValueAsync(int loopCount, int value)
    {
        for (int i = 0; i < loopCount; i++)
        {
            await TestValueTask(0); // 이렇게 바꿔도 ValueTask의 경우 참조 개체가 생성되지는 않음
        }
    }

    static ValueTask<int> TestValueTask(int value)
    {
        return new ValueTask<int>(value);
    }

    private static async Task RefAsync(int loopCount, int value)
    {
        for (int i = 0; i < loopCount; i++)
        {
            await TestTask(0); // Task.FromResult가 참조 개체를 생성하도록 변경
        }
    }

    static Task<int> TestTask(int value)
    {
        return Task.FromResult(value);
    }

    // ValueTask + Delay
    static ValueTask<int> TestTask2(int d)
    {
        var task = Task.Run<int>(async () =>
        {
            await Task.Delay(d);
            return 0;
        });

        return new ValueTask<int>(task);
    }
}