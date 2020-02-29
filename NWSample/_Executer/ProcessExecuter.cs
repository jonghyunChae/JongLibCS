using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Executer
{
    public class ProcessExecuter
    {
        public static string SearchDll(string fileName)
        {
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(currentDir);
            string target = SearchDllChild(dir, fileName);
            if (target != null)
            {
                return target;
            }
            return fileName;
        }

        public static string SearchDllChild(DirectoryInfo dir, string fileName)
        {
            string path = dir.FullName;
            var target = Directory.GetFiles(path, "*.dll")
                    .FirstOrDefault(x => x.EndsWith($"{fileName}.dll"));
            if (target != null)
            {
                return target;
            }

            foreach (var child in dir.GetDirectories())
            {
                target = SearchDllChild(child, fileName);
                if (target != null)
                {
                    return target;
                }
            }
            return null;
        }

        // https://docs.microsoft.com/ko-kr/dotnet/core/tools/dotnet?tabs=netcore21
        // .Net Core CLI 참고
        public static Process GetCoreExecuter(string fileName, params object[] args)
        {
            string file = SearchDll(fileName);
            Console.WriteLine(file);

            string arg = file;
            if (args != null)
            {
                arg = $"{file} {string.Join(" ", args)}";
            }

            var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arg,
                UseShellExecute = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                CreateNoWindow = true
            };
            return process;
        }
    }
}
