using System;
using System.Linq;
using System.Reflection;

namespace Executer
{
    public class ClassExecuter
    {
        public static void Run(Type type, string method, params object[] args)
        {
            var program = Activator.CreateInstance(type);
            type.GetMethod(method).Invoke(program, args);
        }

        public static void Run(string className, string method, params object[] args)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            Type class_type = assembly.GetTypes().FirstOrDefault(x => x.Name == className);
            if (class_type == null)
            {
                throw new Exception("ClassType is cannot Find! : " + className);
            }

            var program = Activator.CreateInstance(class_type);
            class_type.GetMethod(method).Invoke(program, args);
        }
    }
}
