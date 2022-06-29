using System;
using System.Diagnostics;

namespace CSharpSample._97_Process
{
    class Process2
    {
        static void Main()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                }
            };

            process.OutputDataReceived += ReadOutput;
            process.Start();
            process.BeginOutputReadLine();

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "-1")
                {
                    process.Kill();
                    break;
                }
                process.StandardInput.WriteLine(command);
            }

            process.WaitForExit();
            process.Close();
            Console.WriteLine("end");
        }

        static void ReadOutput(object sender, DataReceivedEventArgs e)
        {
            Process process = sender as Process;
            if (process != null)
            {
            }
            Console.WriteLine(e.Data);
        }
    }
}
