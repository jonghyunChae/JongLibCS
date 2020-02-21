using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._4_LINQ
{
    public static class LINQ_EXTEND
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> list, Action<T> act)
        {
            Console.WriteLine("ForEach");
            foreach (var i in list)
            {
                act(i);
                yield return i;
            }
        }
    }

    class LINQ3
    {
        public static void Main()
        {
            var nums = Enumerable.Range(1, 1000000)
                .Select(x => -x)
                .ForEach(x => x++)
                .Where(x => x % 2 == 0)
                .Select(x => (long)x)
                .AsParallel();

            Console.WriteLine("Start Logic");
            Console.WriteLine(nums.Sum());
            Console.WriteLine("End Logic");
        }
    }
}
