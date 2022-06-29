using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample.Generic
{
    class Generic02
    {
        class ABC : IComparable
        {
            public ABC()
            {
            }
            public ABC(int v)
            {
                value = v;
            }

            public int value;
            public int CompareTo(object obj)
            {
                return value.CompareTo(((ABC)obj).value);
            }
        }

        public static object Max(IComparable a, IComparable b)
        {
            return a.CompareTo(b) < 0 ? b : a;
        }

        public static T Max<T>(T a, T b) where T : IComparable, new()
        {
            // 기본적으로 object로 할 수 있는 연산만 가능
            T p = new T();
            return a.CompareTo(b) < 0 ? b : a;
        }

        public static void IsClass<T>(T a) where T : class
        {
            Console.WriteLine("ClassType");
        }

        public static void IsStruct<T>(T a) where T : struct
        {
            Console.WriteLine("StructType");
        }

        public static void Main()
        {
            var m = Max(new ABC(2), new ABC(1));
            Console.WriteLine(m.value);

            IsClass(new object());
            IsStruct(1);
            Console.WriteLine();
        }
    }
}
