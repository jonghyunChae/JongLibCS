using System;
using System.Collections;               // 1. non generic collections

namespace CSharpSample._3_Collection
{
    class Collection1
    {
        public static void Main()
        {
            ArrayList list = new ArrayList();
            list.Add(1);
            list.Add("abc");

            int v1 = (int)list[0];
            Console.WriteLine(v1);

            string v2 = (string)list[1];
            Console.WriteLine(v2);
        }
    }
}
