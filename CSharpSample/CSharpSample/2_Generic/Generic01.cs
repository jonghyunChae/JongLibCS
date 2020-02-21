using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample.Generic
{
    class Point<T>
    {
        public T x;
        public T y;
        public Point(T t1)
        {
            x = default(T);
            y = default(T);
        }
    }

    class Generic01
    {
        public static void Main()
        {
            var p1 = new Point<int>(1);
            Console.WriteLine(p1.x);
            Foo(p1);
            var p2 = new Point<Point<int>>(p1);
            Console.WriteLine(p2.x);
            Foo(p2);
            
            //Foo<object>(p2);
            //Foo(new object());
            //Foo<object>(new object());
        }

        public static void Foo(object n) { Console.WriteLine(">>object"); }
        public static void Foo<T>(T n) { Console.WriteLine(typeof(T).Name); }
    }
}
