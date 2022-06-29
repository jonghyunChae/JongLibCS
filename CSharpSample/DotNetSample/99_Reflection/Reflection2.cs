using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.IO;

namespace CSharpSample._99_Reflection
{
    class Reflection2
    {
        class AAttr : Attribute { }
        class BAttr : Attribute { }

        class ActivatorFuncAttribute : Attribute
        {
            public string name;
            public ActivatorFuncAttribute(string n)
            {
                name = n;
            }
        }

        [AAttr]
        [BAttr]
        class Activator
        {
            public Activator(int v)
            {
                Value = v;
            }
            public int Value;
            public int Prop { get; set; }

            [ActivatorFunc("1")]
            public void First()
            {
                Console.WriteLine("First : " + Value);
            }

            [ActivatorFunc("2")]
            public void Second()
            {
                Console.WriteLine("Second : " + Value);
            }

            public void Third(int i)
            {
                Console.WriteLine("Third : " + i);
            }
        }

        public static void Main()
        {
            Foo();

            Activator a = new Activator(5);
            var cmd = Console.ReadLine();
            Type type = a.GetType();
            foreach (var m in type.GetMethods())
            {
                var attr = m.GetCustomAttribute(typeof(ActivatorFuncAttribute)) as ActivatorFuncAttribute;
                if (attr != null)
                {
                    if (attr.name == cmd)
                    {
                        m.Invoke(a, null);
                    }
                    Console.WriteLine(attr.name);
                }
            }

        }

        public static void Foo([CallerFilePath] string filePath = null,
                                [CallerLineNumber] int n = 0,
                                [CallerMemberName] string name = null)
        {
            Console.WriteLine();
            Console.WriteLine(filePath);
            Console.WriteLine(n);
            Console.WriteLine(name);
            Console.WriteLine();
        }
    }
}
