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
            // ex : Client.Sample0
            string className = "Client.";
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
