using System;
using System.Reflection;

namespace Executer
{
    public class ClassExecuter
    {
        public static void Run(Type type, string method)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Type t = assembly.GetType(type.FullName);

            var program = Activator.CreateInstance(t);
            t.GetMethod(method).Invoke(program, null);
        }

        public static void Run(string typeName, string method)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Type t = assembly.GetType(typeName);

            var program = Activator.CreateInstance(t);
            t.GetMethod(method).Invoke(program, null);
        }
    }
}
