using System;
using System.Threading;

namespace CSharpSample._9_Lock
{
    class Lock6
    {
        static long Value = 0;
        public static void Main()
        {
            Console.WriteLine("Main Start");

            DateTime dt = DateTime.Now;
            Thread[] thlist = new Thread[8];
            for (int i = 0; i < 8; ++i)
            {
                thlist[i] = new Thread(Func);
            }

            for (int i = 0; i < 8; ++i)
            {
                thlist[i].Start(i);
            }

            foreach (var th in thlist)
            {
                th.Join();
            }

            Console.WriteLine(Value);
            Console.WriteLine($"Main End : {(DateTime.Now - dt).TotalMilliseconds}");
            Console.ReadKey();
        }

        public static volatile bool is_lock = false;
        public static void MyLock()
        {
            while (is_lock);
            is_lock = true;
        }

        public static void MyUnLock()
        {
            is_lock = false;
        }

        public static int is_atomic_lock = 0;
        public static void MyAtomicLock()
        {
            while (0 != Interlocked.Exchange(ref is_atomic_lock, 1))
            { }
        }

        public static void MyAtomicUnLock()
        {
            Interlocked.Exchange(ref is_atomic_lock, 0);
        }

        static SpinLock s1 = new SpinLock();
        public static void MySpinLock()
        {
            bool is_spin_lock = false;
            s1.Enter(ref is_spin_lock);
        }

        public static void MySpinUnLock()
        {
            s1.Exit();
        }

        public static void Func(object obj)
        {
            for (int i = 0; i < 50000000; ++i)
            {
                Value += 2;
                //Interlocked.Increment(ref Value);                
            }

            Console.WriteLine(obj + " End");
        }
    }
}
