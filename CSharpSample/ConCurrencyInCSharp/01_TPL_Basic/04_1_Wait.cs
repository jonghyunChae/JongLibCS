using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic;

internal class _04_wait
{
    static async Task Main()
    {
        await AsyncInterleaved();
    }

    static int PrintAndReturn(int value)
    {
        Console.WriteLine($"Do {value}");
        return value;
    }

    static async Task WhenAnySample()
    {
        Task<int> task1 = Task.Delay(2000).ContinueWith(_ => PrintAndReturn(1));
        Task<int> task2 = Task.Delay(1000).ContinueWith(_ => PrintAndReturn(2));

        Task<Task<int>> whenAnyTask = Task.WhenAny(task1, task2);
        Task<int> completeTask = await whenAnyTask;
        int result = await completeTask;
        Console.WriteLine(result);
    }

    // 뒤에 짧은 태스크가 있음에도 순서대로 진행. 
    static async Task AsyncSequential()
    {
        Task<int> task1 = Task.Delay(3000).ContinueWith(_ => PrintAndReturn(1));
        Task<int> task2 = Task.Delay(1000).ContinueWith(_ => PrintAndReturn(2));
        Task<int> task3 = Task.Delay(2000).ContinueWith(_ => PrintAndReturn(3));
        Task<int>[] tasks = new Task<int>[] { task1, task2, task3 };

        foreach (var task in tasks)
        {
            var result = await task;
            Console.WriteLine(result);
        }
    }

    // 빨리 끝난 것 부터 결과를 수행
    static async Task AsyncImmediately()
    {
        Task<int> task1 = Task.Delay(3000).ContinueWith(_ => PrintAndReturn(1));
        Task<int> task2 = Task.Delay(1000).ContinueWith(_ => PrintAndReturn(2));
        Task<int> task3 = Task.Delay(2000).ContinueWith(_ => PrintAndReturn(3));
        Task<int>[] tasks = new Task<int>[] { task1, task2, task3 };

        // 이 때 수행
        Task[] processingTasks = tasks.Select(async t =>
        {
            var result = await t;
            Console.WriteLine("Result " + result);
        }).ToArray();

        await Task.WhenAll(processingTasks);
    }

    // https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
    public static Task<Task<T>>[] Interleaved<T>(IEnumerable<Task<T>> tasks)
    {
        var inputTasks = tasks.ToList();

        var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
        var results = new Task<Task<T>>[buckets.Length];
        for (int i = 0; i < buckets.Length; i++)
        {
            buckets[i] = new TaskCompletionSource<Task<T>>();
            results[i] = buckets[i].Task;
        }

        int nextTaskIndex = -1;
        Action<Task<T>> continuation = completed =>
        {
            var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
            bucket.TrySetResult(completed);
        };

        foreach (var inputTask in inputTasks)
            inputTask.ContinueWith(continuation, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

        return results;
    }

    static async Task AsyncInterleaved()
    {
        Task<int> task1 = Task.Delay(3000).ContinueWith(_ => PrintAndReturn(1));
        Task<int> task2 = Task.Delay(1000).ContinueWith(_ => PrintAndReturn(2));
        Task<int> task3 = Task.Delay(2000).ContinueWith(_ => PrintAndReturn(3));
        Task<int>[] tasks = new Task<int>[] { task1, task2, task3 };

        foreach (Task<Task<int>> bucket in Interleaved(tasks))
        {
            Task<int> t = await bucket;
            var result = await t;
            Console.WriteLine("Result " + result);
        }
    }
}
