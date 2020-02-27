using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._4_LINQ
{
    class LINQ1
    {
        public static void Main()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> nums = numbers
                .Where(Checker)
                .Select(x => -x);

            Console.WriteLine("Start Loop");
            foreach(var i in nums)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("End Loop");
        }

        static bool Checker(int v)
        {
            Console.WriteLine("Checker");
            return v > 5;
        }
    }
}
