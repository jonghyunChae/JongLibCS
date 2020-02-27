using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._6_Disposal
{
    class Disposal1
    {
        public static void Main()
        {
            try
            {
                throw new Exception("Error!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Catch : " + e);
            }
            finally
            {
                Console.WriteLine("Finally");
            }
        }
    }
}
