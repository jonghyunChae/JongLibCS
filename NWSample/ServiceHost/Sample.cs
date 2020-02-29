using Executer;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ServiceHost
{
    public class Sample
    {
        List<Process> processes = new List<Process>();
        void StartServer(string className)
        {
            Console.WriteLine("Start Server : " + className);
            Process process = ProcessExecuter.GetDotNetCoreProcess("Server", className);
            process.Start();
            processes.Add(process);
        }

        void StartClient(string className)
        {
            Console.WriteLine("Start Client : " + className);
            Process process = ProcessExecuter.GetDotNetCoreProcess("Client", className);
            process.Start();
            processes.Add(process);
        }

        public void Run()
        {
            Console.WriteLine("사용할 샘플 번호를 입력하세요. ");
            string className = "Sample" + Console.ReadLine();
            StartServer(className);
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("0 : 종료 대기");
                Console.WriteLine("1 : 클라이언트 생성 (한 개)");
                Console.WriteLine("2 : 클라이언트 생성 (여러 개)");
                Console.Write("Input>> ");

                string cmd = Console.ReadLine().Trim();
                if (cmd == "0")
                    break;

                if (cmd == "1")
                {
                    StartClient(className);
                }

                if (cmd == "2")
                {
                    Console.Write("몇 개 만드시겠습니까? >> ");
                    var n = Console.ReadLine();
                    if (int.TryParse(n, out int count))
                    {
                        for (int i = 0; i < count; ++i)
                            StartClient(className);
                    }
                    else
                    {
                        Console.WriteLine("개수 입력에 실패했습니다.");
                    }
                }
            }

            foreach (var process in processes)
            {
                process.WaitForExit();
            }
            Console.WriteLine("Execute Closed");
        }

    }

}
