using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._98_Pointer
{
    class Pointer1
    {
        class Test
        {
            public int v;
        }

        public static void Main()
        {
            Test t = new Test();
            unsafe
            {
                fixed (int* p = &t.v)
                {
                    //Console.WriteLine(p); // error

                    int n = (int)p;
                    Console.WriteLine($"{n:X}"); // 16 진수 출력
                }
            }

            IntPtr ptr = IntPtr.Zero;
            ptr = Marshal.AllocHGlobal(100);
            WriteToMemory(ptr);
            ReadToMemory(ptr);
            Marshal.FreeHGlobal(ptr);
        }

        static unsafe void WriteToMemory(IntPtr ptr)
        {
            int* v = (int*)ptr.ToPointer();
            for (int i = 0; i < 10; ++i)
            {
                v[i] = i;
            }
        }

        static unsafe void ReadToMemory(IntPtr ptr)
        {
            int* v = (int*)ptr.ToPointer();
            for (int i = 0; i < 10; ++i)
            {
                Console.WriteLine(v[i]);
            }
        }
    }
}
