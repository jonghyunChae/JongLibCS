using Executer;
using System;

namespace Server
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
