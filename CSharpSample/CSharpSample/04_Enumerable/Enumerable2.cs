using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._3_Enumerable
{
    class Enumerable2
    {
        class ABC
        {
            public int Value;
        }

        public static void Main()
        {
            var v = 1;
            IEnumerable<object> e = Func(v);
            v = -1;

            //var v = new ABC();
            //IEnumerable<object> e = Func2(v);
            //v.Value = -1;

            Console.WriteLine("Start Loop");
            foreach(var i in e)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("End Loop");
        }

        static IEnumerable<object> Func(int v)
        {
            Console.WriteLine("Func Start : " + v);
            yield return "abc";
            yield return 2;
            yield return 1.54;
            Console.WriteLine("Func End : " + v);
        }


        static IEnumerable<object> Func2(ABC v)
        {
            Console.WriteLine("Func2 Start : " + v.Value);
            yield return "abc";
            yield return 2;
            yield return 1.54;
            Console.WriteLine("Func2 End : " + v.Value);
        }

        static IEnumerable<object> Func3(int v)
        {
            Console.WriteLine("Func3 Start : " + v);
            if (v < 0)
            {
                return null;
            }
            return Func(v);
        }
    }
}
