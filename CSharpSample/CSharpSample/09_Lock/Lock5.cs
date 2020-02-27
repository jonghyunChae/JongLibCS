using System;
using System.Threading;

namespace CSharpSample._9_Lock
{
    class Lock5
    {
        public class Singleton
        {
            private Singleton()
            {
                Console.WriteLine("SingleTon Create");
            }

            static Singleton instance;
            public static Singleton Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                    return instance;
                }
            }

            public void func()
            {
            }
        }

        public class Singleton2
        {
            private Singleton2()
            {
                Console.WriteLine("SingleTon2 Create");
            }

            static object is_lock = new object();
            static Singleton2 instance;
            public static Singleton2 Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (is_lock)
                        {
                            if (instance == null)
                            {
                                instance = new Singleton2();
                                // DCLP(Dobule Check Locking Pattern)
                                // 숨겨진 결함이 있다.
                                // Thread.MemoryBarrier();
                            }
                        }
                    }
                    return instance;
                }
            }

            public void func()
            {
            }
        }

        [ThreadStatic]
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

        public static void Func(object obj)
        {
            Singleton2.Instance.func();
            for (int i = 0; i < 50000000; ++i)
            {
                Value += 2;
            }

            Console.WriteLine(obj + " End. Value = " + Value);
        }
    }
}
