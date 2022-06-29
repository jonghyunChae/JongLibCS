using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample
{
    class Button
    {
        public event Action click = null;

        public void setClick(Action ac)
        {
            // 내부에서는 =이 된다.
            click = ac;
        }

        // 외부에서는 += -=만 사용가능
        // 원리 add_xxx(), remove_xxx() 함수로 생성 해줌

        public void press()
        {
            click();
        }
    }

    class Program
    {
        public static void Foo() { Console.WriteLine($"Foo "); }
        public static void Goo() { Console.WriteLine($"Goo "); }
        static void Main(string[] args)
        {
            Button b = new Button();
            b.click += Foo;
            b.click += Goo;
            // b.click = Foo;       // 실수 방지
            // b.click(); // 외부에서 호출 불가
            b.press();
        }
    }
}
