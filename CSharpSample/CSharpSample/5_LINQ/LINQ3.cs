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
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> nums = numbers
                .Select(x => -x)
                .ForEach(x => Console.WriteLine(x))
                .Where(x => x > 5);

            Console.WriteLine("Start Logic");
            foreach(var i in nums)
            {
            }
            Console.WriteLine("End Logic");
        }
    }
}
