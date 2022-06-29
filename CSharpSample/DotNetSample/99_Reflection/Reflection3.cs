using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.IO;
namespace CSharpSample._99_Reflection
{
    class Reflection3
    {
        public static void Main()
        {
            dynamic o;

            o = CreateA();
            Console.WriteLine(o.Value);
            o.Value++;
            Console.WriteLine(o.Value);

            o = CreateB();
            Console.WriteLine(o.Value);
            o.Value += "kkk";
            Console.WriteLine(o.Value);
        }

        class A
        {
            public int Value = 10;
        }

        class B
        {
            public string Value = "abc";
        }

        static dynamic CreateA()
        {
            return new A();
        }

        static dynamic CreateB()
        {
            return new B();
        }
    }
}
