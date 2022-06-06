using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._02_AsyncEnumerable
{
    internal class _01_Sample
    {
        public async static Task Main()
        {
            await foreach (var value in GetStringsAsync())
            {
                Console.WriteLine(value);
            }
        }

        static async IAsyncEnumerable<int> GetValuesAsync()
        {
            yield return 1;
            await Task.Delay(100);
            yield return 2;
            await Task.Delay(100);
            yield return 13;
        }

        static async IAsyncEnumerable<string> GetStringsAsync()
        {
            using HttpClient http = new();
            var res = await http.GetAsync(@"https://www.naver.com");
            yield return @"https://www.naver.com";
            var content = await res.Content.ReadAsStringAsync();
            var strs = await Task.Run(() => content.Split('\n'));
            foreach (var str in strs)
            {
                yield return str;
            }
        }
    }
}
