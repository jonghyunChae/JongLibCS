using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._3_Collection
{
    class Collection4
    {
        class Test
        {
            public Test(int v1, int v2)
            {
                Value = v1;
                Value2 = v2;
            }

            public int Value;
            public int Value2;
            public override string ToString()
            {
                return "abc";
            }
            public override bool Equals(object obj)
            {
                Console.WriteLine("Equals");
                if (obj == this) return true;
                if (obj is Test)
                {
                    Test t = (Test)obj;
                    return Value == t.Value; // && Value2 == t.Value2;
                }
                return false;
            }

            public override int GetHashCode()
            {
                Console.WriteLine("GetHashCode");
                return Value;
            }
        }

        public class MyIgnoreCaseStringComparer : IEqualityComparer<string> // IEqualityComparer
        {
            public bool Equals(string x, string y)
            {
                return x.ToLower().Equals(y.ToLower());
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLower().GetHashCode();
            }
        }

        public static void Main()
        {
            var TestSet = new HashSet<Test>();
            TestSet.Add(new Test(1, 1));

            if (TestSet.Contains(new Test(1, 2)))
            {
                Console.WriteLine("Contains");
            }

            var TestSet2 = new HashSet<string>(new MyIgnoreCaseStringComparer());
            TestSet2.Add("abc");
            if (TestSet2.Contains("ABC"))
            {
                Console.WriteLine("Contains2");
            }

        }
    }
}
