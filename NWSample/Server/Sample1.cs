using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Sample1
    {
        public void Run()
        {
            Console.WriteLine("Input For Accept");
            Console.ReadKey();

            using (Socket listenSock 
                = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress hostIP = IPAddress.Loopback;
                var endPoint = new IPEndPoint(hostIP, 4444);
                listenSock.Bind(endPoint);
                listenSock.Listen(-1);
                Console.WriteLine("Listen!");

                List<Socket> clientSocks = new List<Socket>();
                for (int i = 0; i < 2; ++i)
                {
                    Socket clientSock = listenSock.Accept();
                    clientSocks.Add(clientSock);
                    Console.WriteLine("Accept!");
                }

                foreach (var sock in clientSocks)
                {
                    sock.Close();
                }
                clientSocks.Clear();
                Console.WriteLine("Close Socket!");
            }

            Console.WriteLine("End Server!");
            Console.ReadKey();
        }
    }
}
