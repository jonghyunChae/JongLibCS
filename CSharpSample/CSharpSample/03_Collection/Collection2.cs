using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._3_Collection
{
    class Collection2
    {
        static void Main()
        {
            var li = new List<int>();
            li.Add(1);
            li.Add(2);
            li.Add(3);

            IReadOnlyList<int> rlist = li;
            //rlist.Add()

            ICollection<int> clist = li;
            Console.WriteLine(clist.Count);
        }
    }
}
