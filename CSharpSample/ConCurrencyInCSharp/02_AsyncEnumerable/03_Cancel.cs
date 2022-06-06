using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._02_AsyncEnumerable;

internal class _03_Cancel
{
    static async Task Main()
    {
        var cts = new CancellationTokenSource(400);
        var token = cts.Token;
        var v = await SlowRange(token).FirstAsync();
        Console.WriteLine(v);

        Thread.Sleep(1000);
        Console.WriteLine(token.IsCancellationRequested);
        await foreach (var t in SlowRange().WithCancellation(token).ConfigureAwait(false))
        {
            Console.WriteLine(t);
        }

        await foreach (var t in SlowRange(token))
        {
            Console.WriteLine(t);
        }
    }

    static async IAsyncEnumerable<int> SlowRange(
        [EnumeratorCancellation] CancellationToken token = default)
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(i * 100, token);
            yield return i;
        }
    }
}
