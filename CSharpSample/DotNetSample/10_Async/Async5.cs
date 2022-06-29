using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpSample._10_Async
{
    class Async5
    {
        static void Main()
        {
            string contents = "HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello";
            WriteLog(contents);
            Console.ReadKey();
        }

        static void WriteLog(string log)
        {
            string fileName = "a.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            Task task = null;
            using (FileStream file = new FileStream(fileName, FileMode.OpenOrCreate))
            using (StreamWriter writer = new StreamWriter(file))
            {
                task = writer.WriteLineAsync(log);
            }

            task.ContinueWith(t =>
           {
               Console.WriteLine("WriteFile Complete");
           });

            Console.WriteLine("WriteLog Wait..");
        }

    }
}
