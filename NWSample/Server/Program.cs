using Executer;
using System;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            // ex : Server.Sample0
            string className = "Server.";
            if (args.Length <= 0)
            {
                Console.WriteLine("Arg count is zero");
                className += "Sample";
            }
            else
            {
                className += args[0];
            }

            Console.WriteLine("Run >> " + className);
            ClassExecuter.Run($"{className}", "Run");
            Console.ReadKey();
        }
    }
}
