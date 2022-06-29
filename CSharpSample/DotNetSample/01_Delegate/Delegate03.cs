using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._01_Delegate
{
    // Generic 과 관련된 주제.

    // A(Dog)가 B(Animal)로 변환 될수 있을때

    // X<A> 가 X<B>로 변환 될수 있다면 "공변성(Covariance)"를 지원한다고 표현
    // X<B> 가 X<A>로 변환 될수 있다면 "반변성(contrariance)"를 지원한다고 표현

    // 공변성. delegate만들때 반환 타입에서 사용 - out
    // 반변성. delegate만들때 함수 인자에서 사용 - in
    /*
    // 공변성 ( Covariance )
    delegate TResult Func<out TResult>();

    // 반변성 ( contravariance )
    delegate void Action<in T>(T arg);
    */

    class Animal { }
    class Dog : Animal { }

    class Delegate03
    {
        public static Animal F1() { return new Animal(); }
        public static Dog F2() { return new Dog(); }

        public static void Main()
        {
            Func<Animal> d1 = F1;
            Func<Dog> d2 = F2;

            Func<Animal> d3 = d1; // ok. d3와 d1은 다른 타입이다.
            Func<Animal> d4 = d2; // ?

            Animal ret = d4();  // ? 채워 보세요. 
                                // 이때 d4가 반드시 Animal 을 리턴해야만 할까 ?
                                // Dog를 리턴하면 안될까 ?


            Action<Animal> a1 = F3;
            Action<Dog> a2 = F4;

            Action<Dog> a3 = a2; // ok.. 동일 타입.
            Action<Dog> a4 = a1;

            Animal dog = new Dog();
            //a4((Dog)dog);   // a4는 Dog를 인자로 보내게 된다.
                            // a4의 인자타입인 반드시 Dog참조이어야 할까 ?
                            // Animal 참조이면 안될까?

            //Animal[] animals = new Dog[1];
            //animals[0] = new Dog();
            //F5(animals);
            //F6(animals);
        }

        public static void F3(Animal a) { Console.WriteLine(a); }
        public static void F4(Dog a) { Console.WriteLine(a); }

        public static void F5(Animal[] a) { Console.WriteLine(a[0]); }
        public static void F6(Dog[] a) { Console.WriteLine(a[0]); }


    }
}
