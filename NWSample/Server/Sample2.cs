using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    // tcp send & recv
    public class Sample2
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

                while (true)
                {
                    Socket clientSock = listenSock.Accept();
                    clientSock.ReceiveBufferSize = 512;
                    //clientSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 512);
                    Console.WriteLine($"Accept Socket! {clientSock.RemoteEndPoint}");
                    byte[] buffer = new byte[1024];
                    while (true)
                    {
                        Array.Clear(buffer, 0, buffer.Length);

                        Console.WriteLine("\nReceive Start!");
                        int recvByte = 0;
                        try
                        {
                            recvByte = clientSock.Receive(buffer);
                            Console.WriteLine($"Receive Byte : {recvByte}");
                            if (recvByte == 0)
                            {
                                break;
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine($"Socket Error : {ex}");
                            break;
                        }

                        string contents = System.Text.Encoding.Unicode.GetString(buffer, 0, recvByte);
                        Console.WriteLine($"Contents : {contents.TrimEnd()}");
                    }

                    clientSock.Close();
                    Console.WriteLine("Close Socket!");
                }
            }

            Console.WriteLine("End Server!");
            Console.ReadKey();
        }
    }
}
