using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Sample1
    {
        public void Run()
        {
            Console.WriteLine("Input For Connect");
            Console.ReadKey();

            using (Socket socket
                = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress hostIP = IPAddress.Loopback;
                var endPoint = new IPEndPoint(hostIP, 4444);
                Console.WriteLine("Connect!");

                socket.Connect(endPoint);
                Console.WriteLine("Connect Success!");

                Console.ReadKey();
                Console.WriteLine("Close Socket!");
            }

            Console.WriteLine("End Server!");
            Console.ReadKey();
        }
    }
}
