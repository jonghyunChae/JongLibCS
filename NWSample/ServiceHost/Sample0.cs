using Executer;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ServiceHost
{
    public class Sample0
    {
        public void Run()
        {
            List<Process> processes = new List<Process>();
            processes.Add(ProcessExecuter.GetCoreExecuter("Client", "Sample0"));
            processes.Add(ProcessExecuter.GetCoreExecuter("Server", "Sample0"));
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
        
    }
}
