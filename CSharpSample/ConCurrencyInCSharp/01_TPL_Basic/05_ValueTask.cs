using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic;

internal class _05_ValueTask
{
    static int ReturnValue;
    static ValueTask<int> GetValue()
    {
        Console.WriteLine("Do Job");
        ReturnValue = Interlocked.Increment(ref ReturnValue);
        return new ValueTask<int>(ReturnValue);
    }

    static async Task Main()
    {
        ValueTask<int> v1 = GetValue();
        Task<int> task = v1.AsTask();

        int result = await v1;
        Console.WriteLine(v1);

        int otherResult = await v1;
        Console.WriteLine(otherResult);

        Console.WriteLine(ReturnValue);

        result = await task;
        Console.WriteLine(result);

        otherResult = await task;
        Console.WriteLine(otherResult);
    }
}
