using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    // tcp send & recv
    public class Sample2
    {
        string hugeMessage = "";
        public void Run()
        {
            Console.WriteLine("Input For Connect");
            Console.ReadKey();
            for (int i = 0; i < 65535; ++i)
            {
                hugeMessage += "a";
            }

            using (Socket socket
                = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPAddress hostIP = IPAddress.Loopback;
                var endPoint = new IPEndPoint(hostIP, 4444);
                Console.WriteLine("Connect!");

                socket.Connect(endPoint);
                Console.WriteLine($"Connect Success! {socket.LocalEndPoint}");

                while(true)
                {
                    Console.Write("\nInput Contents (close:-1, huge message:-2) > ");
                    string contents = Console.ReadLine();
                    if (string.IsNullOrEmpty(contents))
                    {
                        continue;
                    }
                    if (contents == "-1")
                    {
                        break;
                    }
                    if (contents == "-2")
                    {
                        contents = hugeMessage;
                    }

                    byte[] buffer = System.Text.Encoding.Unicode.GetBytes(contents);
                    Console.WriteLine($"Send! {buffer.Length}");
                    socket.Send(buffer);
                    Console.WriteLine("Success!");
                }

                Console.WriteLine("Close Socket!");
            }

            Console.WriteLine("End Server!");
            Console.ReadKey();
        }
    }
}
