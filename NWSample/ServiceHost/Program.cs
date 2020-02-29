using Executer;
using System;
using System.Reflection;

namespace ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassExecuter.Run(typeof(Sample0), "Run");
            Console.ReadKey();
        }
    }
}
