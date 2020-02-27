using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._3_Enumerable
{
    class Enumerable1
    {
        public static void Main()
        {
            //foreach (var i in Func(0))
            //{
            //    Console.WriteLine(i);
            //}

            IEnumerator<object> e = Func(0).GetEnumerator();
            while(e.MoveNext())
            {
                Console.WriteLine(e.Current);
            }
        }

        static IEnumerable<object> Func(int v)
        {
            yield return "abc";
            if (v < 0)
            {
                yield break;
            }
            yield return 2;
            yield return 1.54;
        }
    }
}
