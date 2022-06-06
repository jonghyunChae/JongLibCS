using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConCurrencyInCSharp._01_TPL_Basic;

internal class _03_Exception
{
    static async Task NotImplementException()
    {
        throw new NotImplementedException();
    }

    static async Task InvalidOperationException()
    {
        throw new InvalidOperationException();
    }

    static async Task Main()
    {
        var task1 = NotImplementException();
        var task2 = InvalidOperationException();

        Task whenAll = Task.WhenAll(task1, task2);
        try
        {
            Console.WriteLine("Start await");
            await whenAll;
        }
        catch (Exception e)
        {
            Console.WriteLine("Flatten");
            Console.WriteLine(whenAll.Exception.Flatten());

            Console.WriteLine("Inner Exception");
            Exception ex = e;
            while (ex != null)
            {
                Console.WriteLine(ex);
                ex = ex.InnerException;
            }
        }
    }
}
