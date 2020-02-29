using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._97_Process
{
    class Process1
    {
        static void Main()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = false
                }
            };
            process.Start();
            process.WaitForExit();
            Console.WriteLine("Process End");
            Console.ReadKey();
        }
    }
}
