using System.Collections.Concurrent;

namespace ConCurrencyInCSharp._04_TaskScheduler;

internal class _01_TaskScheduler
{

    internal sealed class SingleThreadTaskScheduler : TaskScheduler
    {
        static Thread _thread;
        static BlockingCollection<WorkItem> _workItems;

        class WorkItem
        {
            Task _task;
            public Task Task => _task;
            Func<Task, bool> _func;
            public Func<Task, bool> Func => _func;

            internal WorkItem(Func<Task, bool> func, Task task)
            {
                _task = task;
                _func = func;
            }
        }

        static SingleThreadTaskScheduler()
        {
            _workItems = new BlockingCollection<WorkItem>();

            _thread = new Thread(threadFunc);
            _thread.IsBackground = true;
            _thread.Start();
        }

        static void threadFunc()
        {
            while (true)
            {
                var item = _workItems.Take();
                item.Func(item.Task);
            }
        }

        protected override void QueueTask(Task task)
        {
            Console.WriteLine("QueueTask! ");
            _workItems.Add(new WorkItem(TryExecuteTask, task));
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override IEnumerable<Task>? GetScheduledTasks() => null;
        /// </summary>
        public override int MaximumConcurrencyLevel => 1;
    }


    static async Task<int> Main(string[] args)
    {
        var staScheduler = new SingleThreadTaskScheduler();
        await Task.Factory.StartNew(async () =>
        {
            await MainTask();
        }, CancellationToken.None, TaskCreationOptions.AttachedToParent, staScheduler).Unwrap();

        Thread.Sleep(1000);
        return 1;

        async Task MainTask()
        {
            Console.WriteLine("Main Task!");
            
            _ = LongTimeTask();
            Console.WriteLine("AfterLongTimeTask Return");

            await Task.Delay(10);

            Console.WriteLine("After Delay");
        }

        async Task LongTimeTask()
        {
            Console.WriteLine("LongTimeTaskBegin " + Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(100);

            Console.WriteLine("LongTimeTask Before Await1 " + Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(100);

            Console.WriteLine("LongTimeTask Before Await2 " + Thread.CurrentThread.ManagedThreadId);

            await Task.Delay(100);

            Console.WriteLine("LongTimeTaskEnd " + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
