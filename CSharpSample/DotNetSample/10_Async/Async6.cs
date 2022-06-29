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
    class Async6
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Loop Start");
                FuncAwait();
                Thread.Sleep(3000);
                Console.WriteLine("Loop End");
                Console.ReadKey();
            }
        }

        static async void DoHeavyWork()
        {
            await Task.Delay(3000);
            throw new Exception("DoHeavyWork Error!");
        }

        static async void FuncAwait()
        {
            try
            {
                DoHeavyWork();
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch Exception! : " + e);
            }
        }

        static async Task DoHeavyWork2()
        {
            await Task.Delay(1000);
            throw new Exception("DoHeavyWork Error!");
        }

        static async void FuncAwait2()
        {
            try
            {
                await DoHeavyWork2();
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch Exception! : " + e);
            }
        }

        static void FuncContinue()
        {
            var task = DoHeavyWork2();
            task.ContinueWith(t =>
            {
                Console.WriteLine("Catch Exception! : " + t.Exception);
            });
        }
    }
}
