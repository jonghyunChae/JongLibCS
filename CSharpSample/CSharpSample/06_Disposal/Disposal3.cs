using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._6_Disposal
{
    class Disposal3
    {
        public static void Main()
        {
            Console.WriteLine("Write");
            Write("a.txt", "hello");
            Console.WriteLine("Read1");
            Read("a.txt");
            Console.WriteLine("Read2");
            Read("a.txt");
            Console.WriteLine("Main end");

        }

        static void Write(string path, string content)
        {
            FileStream file = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine(content);
        }

        static void Read(string path)
        {
            FileStream file = new FileStream(path, FileMode.OpenOrCreate);
            StreamReader reader = new StreamReader(file);
            Console.WriteLine(reader.ReadLine());
        }
    }
}
