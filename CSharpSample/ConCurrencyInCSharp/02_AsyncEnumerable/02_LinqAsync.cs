using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// System.Linq.Async -> https://www.nuget.org/packages/System.Linq.Async/
// ㄴ 필요한 라이브러리

// System.Interactive.Async -> https://www.nuget.org/packages/System.Interactive.Async
// ㄴ 추천 라이브러리
namespace ConCurrencyInCSharp._02_AsyncEnumerable;

internal class _02_LinqAsync
{
    static async Task Main()
    {
        IAsyncEnumerable<int> values = SlowRange().WhereAwait(async value =>
        {
            await Task.Delay(10);
            return value % 2 == 0;
        });

        await foreach (var value in values)
        {
            Console.WriteLine(value);
        }

        values = SlowRange().Where(x => x % 2 == 0).Select(x => x * 2);
        await foreach (var value in values)
        {
            Console.WriteLine(value);
        }

        await foreach (var value in GetValues().ToAsyncEnumerable())
        {
            Console.WriteLine(value);
        }

        Console.WriteLine(await SlowRange().CountAsync());
    }

    static async IAsyncEnumerable<int> SlowRange()
    {
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(i * 100);
            yield return i;
        }
    }

    static IEnumerable<int> GetValues()
    {
        yield return 1;
        yield return 2;
        yield return 3;

    }
}
