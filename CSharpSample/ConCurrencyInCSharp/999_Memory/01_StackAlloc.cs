using System.Collections.Concurrent;

namespace ConCurrencyInCSharp._04_TaskScheduler;

internal class _01_StackAlloc
{
    static void Main(string[] args)
    {
        Span<int> alist = stackalloc int[]{ 1, 2, 3, 4};
        Do(alist);
    }

    static void Do(ReadOnlySpan<int> list)
    {
        // do something
    }
}
