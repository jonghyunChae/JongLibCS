using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using static System.Net.Mime.MediaTypeNames;

namespace Executer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Process> processes = new List<Process>();
            processes.Add(GetCoreExecuter("Client"));
            processes.Add(GetCoreExecuter("Server"));
            foreach (var process in processes)
            {
                process.Start();
            }

            foreach (var process in processes)
            {
                process.WaitForExit();
            }
            Console.WriteLine("Execute Closed");
        }

        static string FindDllPath(string fileName)
        {
            string currentDir = Directory.GetCurrentDirectory();
            string target = SearchDll(currentDir, fileName);
            if (target != null)
            {
                return target;
            }

            foreach (var dir in Directory.GetDirectories(currentDir))
            {
                target = SearchDll(dir, fileName);
                if (target != null)
                {
                    return target;
                }
            }
            return fileName;
        }

        static string SearchDll(string dir, string fileName)
        {
            return Directory.GetFiles(dir, "*.dll")
                    .FirstOrDefault(x => x.EndsWith($"{fileName}.dll"));
        }

        static Process GetCoreExecuter(string fileName)
        {
            string file = FindDllPath(fileName);
            Console.WriteLine(file);

            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = file,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true
            };
            return process;
        }
    }
}
