using Executer;
using System;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(">>" + typeof(Program).Namespace);
            string className = string.Empty;
            if (args.Length <= 0)
            {
                Console.WriteLine("Input Sample Number");
                var number = Console.ReadLine();
                className = $"Sample{number}";
            }
            else
            {
                className = args[0];
            }

            Console.WriteLine("Run >> " + className);
            ClassExecuter.Run(className, "Run");
            Console.ReadKey();
        }
    }
}
