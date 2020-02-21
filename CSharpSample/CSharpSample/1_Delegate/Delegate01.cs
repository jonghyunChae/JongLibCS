using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample
{
    /*
    class FP : MulticastDelegate
    {
    };
    */
    delegate void FP(int a); // FP는 함수가 아닌 함수 타입입니다.

    class Program
    {
        public static void Foo(int a) { }
        public static void Goo(int a) { Console.WriteLine($"Goo : {a}"); }
        static void Main(string[] args)
        {
            FP f1 = new FP(Foo);// 정확한 표현
            FP f2 = Foo;        // 축약 표현. 위와 동일.

            f1.Invoke(10);// 이순간 Foo(10) 이 된다.
            f1(10);       // 위 코드와 동일

            // delegate는 class -> reference type
            Console.WriteLine(f2 == Foo);

            // f2 = f2 + Goo -> 새로운 객체가 생성되는 것
            f2 += Goo;

            Console.WriteLine(f2 == Foo);
        }
    }
}
