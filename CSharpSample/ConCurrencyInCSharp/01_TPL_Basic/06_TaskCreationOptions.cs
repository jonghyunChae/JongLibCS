using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic
{
    internal class _06_TaskCreationOptions
    {
        static void WaitAllTest()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Step 1");
                await Task.Delay(100);
                Console.WriteLine("Step 2");
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        // UnWarp
        static void WaitAllTest1()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Step 1");
                await Task.Delay(100);
                Console.WriteLine("Step 2");
            }).Unwrap());
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        // AttachedToParent
        static void WaitAllTest2()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Step 1");
                Task.Factory.StartNew(() => {
                    Console.WriteLine("Step 2");
                }, TaskCreationOptions.AttachedToParent);
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        // AttachedToParent + Task.Run
        // Task.InternalStartNew(null, action, null, default, TaskScheduler.Default,
        //          TaskCreationOptions.DenyChildAttach, InternalTaskOptions.None)
        // 를 호출하기 때문에 불가
        static void WaitAllTest2_TaskRun()
        {
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() =>
            {
                Console.WriteLine("Step 1");
                Task.Factory.StartNew(() => {
                    Console.WriteLine("Step 2");
                }, TaskCreationOptions.AttachedToParent);
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        static void WaitAllTest3()
        {
            TaskFactory factory = new(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.None);
            var tasks = new List<Task>();
            tasks.Add(factory.StartNew(() =>
            {
                Console.WriteLine("Step 1");
                factory.StartNew(() => {
                    Console.WriteLine("Step 2");
                });
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        static void WaitAllTest3_DenyChildAttach()
        {
            TaskFactory factory = new(TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None);
            var tasks = new List<Task>();
            tasks.Add(factory.StartNew(() =>
            {
                Console.WriteLine("Step 1");
                factory.StartNew(() => {
                    Console.WriteLine("Step 2");
                });
            }));
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Task All Done");
        }

        static async Task Main()
        {
            DoJob(WaitAllTest, nameof(WaitAllTest));
            DoJob(WaitAllTest1, nameof(WaitAllTest1));
            DoJob(WaitAllTest2, nameof(WaitAllTest2));
            DoJob(WaitAllTest2_TaskRun, nameof(WaitAllTest2_TaskRun));
            DoJob(WaitAllTest3, nameof(WaitAllTest3));
            DoJob(WaitAllTest3_DenyChildAttach, nameof(WaitAllTest3_DenyChildAttach));
        }

        static void DoJob(Action action, string comment)
        {
            Console.WriteLine(comment);
            action();
            Thread.Sleep(1000);
            Console.WriteLine();
        }
    }
}
