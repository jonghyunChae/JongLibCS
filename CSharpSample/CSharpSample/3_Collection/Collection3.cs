using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
using System.Collections;               // 1. non generic collections
using System.Collections.Specialized;   // 2. 타입에 특화된 collections
using System.Collections.Generic;       // 3. generic collections
using System.Collections.ObjectModel;   // 4. for Custom collections 
using System.Collections.Concurrent;    // 5. thread safe collections
 */
namespace CSharpSample._3_Collection
{
    class Collection3
    {
        static void Main()
        {
            IDictionary<int, int> adic = new Dictionary<int, int>();
            adic[1] = 2;
            adic.Add(2, 3);
            if (adic.ContainsKey(1))
            {
                Console.WriteLine("ContainsKey");
            }

            if (adic.TryGetValue(1, out int v))
            {
                Console.WriteLine(v);
            }

            adic = new SortedDictionary<int, int>();
            adic[5] = 5;
            adic[3] = 3;
            //adic.Add(3, 3);
            foreach (var pair in adic)
            {
                Console.WriteLine(pair.Key + ", " + pair.Value);
            }

            LinkedList<int> li = new LinkedList<int>();
            li.AddLast(-1);
            li.AddFirst(1);
            foreach(var i in li)
            {
                Console.WriteLine(i);
            }

            HashSet<int> hs = new HashSet<int>();
            hs.Add(1);
            if (hs.Contains(1))
            {
                Console.WriteLine("Contains hash set");
            }

            //hs.Add(1);
        }
    }
}
