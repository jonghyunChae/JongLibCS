using System.Buffers;
using System.Collections.Concurrent;

namespace ConCurrencyInCSharp._04_TaskScheduler;

internal class _02_ArrayPool
{
    static void Main(string[] args)
    {
        var pool = ArrayPool<int>.Shared;
        Span<int> alist = pool.Rent(4);
        Do(alist);
    }

    static void Do(ReadOnlySpan<int> list)
    {
        // do something
    }
}