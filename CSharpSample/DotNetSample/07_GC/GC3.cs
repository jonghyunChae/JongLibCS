using System;

namespace CSharpSample._07_GC
{
    class GC3
    {
        class TestParent
        {
            public TestClass c;
            public TestParent(TestClass c)
            {
                this.c = c;
            }
        }

        class TestClass
        {
            public TestClass()
            {
            }
        }

        static void PrintGCGen(WeakReference wk, string name)
        {
            try
            {
                Console.WriteLine(name + " " + GC.GetGeneration(wk) + " 세대");
            }
            catch (Exception e)
            {
                Console.WriteLine(name + " 사망");
            }
        }

        static void ProcessGC(int gen)
        {
            Console.WriteLine();
            GC.Collect(gen);
            if (gen != 2)
            {
                Console.WriteLine("Process GC " + gen + " Gen");
            }
            else
            {
                Console.WriteLine("Process GC All Gen");
            }
        }

        static void TestFunc(out WeakReference wt, out WeakReference wp)
        {
            TestParent parent = new TestParent(null);
            wp = new WeakReference(parent);

            PrintGCGen(wp, nameof(TestParent));
            GC.Collect();

            PrintGCGen(wp, nameof(TestParent));
            GC.Collect();

            parent.c = new TestClass();
            wt = new WeakReference(parent.c);
            parent = null;

            Console.WriteLine();
            Console.WriteLine("처음 상태");
            Console.WriteLine("(Root) -x- TestParent(2) - TestClass(0)");
            PrintGCGen(wt, nameof(TestClass));      // 0세대
            PrintGCGen(wp, nameof(TestParent));     // 2세대 (null)

            ProcessGC(0);
            PrintGCGen(wt, nameof(TestClass));      // 1세대
            PrintGCGen(wp, nameof(TestParent));     // 2세대 (null)

            ProcessGC(1); // 여기를 2로 바꾸묜?
            PrintGCGen(wt, nameof(TestClass));
            PrintGCGen(wp, nameof(TestParent));

            ProcessGC(2);
            PrintGCGen(wt, nameof(TestClass));
            PrintGCGen(wp, nameof(TestParent));
        }

        static void TestFunc2(out WeakReference wt, out WeakReference wp)
        {
            TestClass tc = new TestClass();
            wt = new WeakReference(tc);

            PrintGCGen(wt, nameof(TestClass));
            GC.Collect();

            PrintGCGen(wt, nameof(TestClass));
            GC.Collect();

            TestParent parent = new TestParent(null);
            wp = new WeakReference(parent);

            parent.c = tc;
            parent = null;
            tc = null;

            Console.WriteLine();
            Console.WriteLine("처음 상태");
            Console.WriteLine("(Root) -x- TestParent(0) - TestClass(2)");
            PrintGCGen(wt, nameof(TestClass));      // 2세대
            PrintGCGen(wp, nameof(TestParent));     // 0세대 (null)

            ProcessGC(0);
            PrintGCGen(wt, nameof(TestClass));
            PrintGCGen(wp, nameof(TestParent));

            ProcessGC(1); // 여기를 2로 바꾸묜?
            PrintGCGen(wt, nameof(TestClass));
            PrintGCGen(wp, nameof(TestParent));

            ProcessGC(2);
            PrintGCGen(wt, nameof(TestClass));
            PrintGCGen(wp, nameof(TestParent));
        }

        static void Main()
        {
            TestFunc(out var wt, out var wp);
        }
    }
}
