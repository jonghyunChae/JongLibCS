using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._01_Delegate
{
    class Delegate04
    {
        public static void Main()
        {
            int a = 2;
            Action act = () => { Console.WriteLine(a++); };

            act();

            a = 10;

            act();
            act();
            act();
        }
    }
}
