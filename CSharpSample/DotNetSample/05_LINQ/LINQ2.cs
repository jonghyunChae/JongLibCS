using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._4_LINQ
{
    class LINQ2
    {
        public static void Main()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> nums = numbers
                .Where(Checker)
                .Select(x => -x)
                .ToList();

            Console.WriteLine("Start Logic");

            Console.WriteLine(nums.Count);
            Console.WriteLine(nums.First());
            nums.Clear();
            Console.WriteLine(nums.FirstOrDefault() == default(int));

            Console.WriteLine("End Logic");
        }

        static bool Checker(int v)
        {
            Console.WriteLine("Checker");
            return v > 5;
        }
    }
}
