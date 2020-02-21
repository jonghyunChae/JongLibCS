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
            // Fluent Syntex
            // Query Syntex
            var nums =
                from n in Enumerable.Range(1, 1000)
                where n > 500
                select -n
                    into n2
                    where n2 % 5 == 0
                    select n2;


            Console.WriteLine("Start Logic");
            foreach(var i in nums)
            {
                Console.WriteLine(i);
            }

            var groups = from n in nums
                     select new { x = n % 2 == 0, y = n }
                     into n2
                     group n2.y by n2.x;

            foreach (var group in groups)
            {
                Console.WriteLine(group.Key + " : " + group.Count());
            }
            Console.WriteLine("End Logic");
        }
    }
}
