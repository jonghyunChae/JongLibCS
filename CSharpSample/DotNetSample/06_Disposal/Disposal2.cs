using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._6_Disposal
{
    class Disposal2
    {
        public class Resource
        {
            public Resource()
            {
                Console.WriteLine("생성");
            }
            ~Resource()
            {
                Console.WriteLine("파괴");
            }
            public void Open()
            {
                Console.WriteLine("자원 획득");
            }
            public void Close()
            {
                Console.WriteLine("자원 반납");
            }
        }

        public static void Main()
        {
            try
            {
                Resource r = new Resource();
                r.Open();
                //throw new Exception("Error!");
                r.Close();
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
