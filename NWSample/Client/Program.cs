using Executer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = typeof(Program).Namespace;
            Console.WriteLine($"{name} arg Len : {args.Length}");
            if (args.Length <= 0)
            {
                throw new Exception("Arg Count is zero!");
            }

            ClassExecuter.Run($"{name}.{args[0]}", "Run");
            Console.ReadKey();
        }
    }
}
