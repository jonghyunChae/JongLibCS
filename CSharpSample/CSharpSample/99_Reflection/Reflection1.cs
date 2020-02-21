using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CSharpSample._99_Reflection
{
    class Reflection1
    {
        class Activator
        {
            public Activator(int v)
            {
                Value = v;
            }
            public int Value;
            public int Prop { get; set; }
            public void First(int i)
            {
                Console.WriteLine("First : " + i);
            }

            public void Second(int i)
            {
                Console.WriteLine("Second : " + i);
            }

            public void Third(int i)
            {
                Console.WriteLine("Third : " + i);
            }
        }

        public static void Main()
        {
            Activator a = new Activator(5);
            MethodInfo method = null;
            Type type = a.GetType();
            method = type.GetMethod("First");
            method.Invoke(a, new object[] { 1 });

            method = type.GetMethod("Second");
            method.Invoke(a, new object[] { 1 });

            method = type.GetMethod("Third");
            method.Invoke(a, new object[] { 1 });

            method = type.GetProperty("Prop").GetSetMethod();
            object result = method.Invoke(a, new object[] { 10 });

            method = type.GetProperty("Prop").GetGetMethod();
            result = method.Invoke(a, null);
            Console.WriteLine((int)result);

            FieldInfo field = type.GetField("Value");//
            result = field.GetValue(a);
            Console.WriteLine((int)result);

            MemberInfo[] member = type.GetMember("Value");//
            Console.WriteLine(member[0]);
        }
    }
}
