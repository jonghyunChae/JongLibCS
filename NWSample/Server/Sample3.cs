using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    // echo server
    // basic of async socket programming 1
    public class Sample3
    {
        static IPAddress hostIP = IPAddress.Loopback;
        const int port = 4444;
        public class AcceptServer
        {
            Queue<Socket> newClients = new Queue<Socket>();
            Thread thread = null;
            bool running = false;
            public void Start()
            {
                thread = new Thread(this.Accept);
                running = true;
                thread.Start();
            }

            public void Close()
            {
                running = false;
                thread.Join();
                thread = null;
            }

            private void Accept()
            {
                using (Socket listenSock
                    = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    var endPoint = new IPEndPoint(hostIP, port);
                    listenSock.Bind(endPoint);
                    listenSock.Listen(-1);

                    Console.WriteLine("Listen!");
                    while (running)
                    {
                        Socket clientSock = listenSock.Accept();
                        Console.WriteLine("Accept Socket By AcceptServer!");
                        lock (newClients)
                        {
                            newClients.Enqueue(clientSock);
                        }
                    }
                }
            }

            public Socket GetNewClient()
            {
                lock (newClients)
                {
                    if (newClients.Count > 0)
                    {
                        Socket newSock = newClients.Dequeue();
                        return newSock;
                    }
                }
                return null;
            }
        }

        public void Run()
        {
            Console.WriteLine("Input For Accept");
            Console.ReadKey();

            AcceptServer accept = new AcceptServer();
            accept.Start();
            Socket clientSock = null;
            while (true)
            {
                clientSock = accept.GetNewClient();
                if (clientSock == null)
                {
                    Thread.Sleep(100);
                    continue;
                }
                else
                {
                    Console.WriteLine($"NewSock >> {clientSock.RemoteEndPoint}");
                }

                byte[] buffer = new byte[1024];
                while (true)
                {
                    Console.WriteLine("\nReceive Start!");
                    int recvByte = 0;
                    try
                    {
                        Array.Clear(buffer, 0, buffer.Length);

                        recvByte = clientSock.Receive(buffer);
                        Console.WriteLine($"Receive Byte : {recvByte}");
                        if (recvByte == 0)
                        {
                            break;
                        }

                        clientSock.Send(buffer, recvByte, SocketFlags.None);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"Socket Error : {ex}");
                        break;
                    }
                }

                Console.WriteLine($"Close Socket! >> {clientSock.RemoteEndPoint}");
                clientSock.Close();
                clientSock = null;
            }
            accept.Close();

            Console.WriteLine("End Server!");
            Console.ReadKey();
        }
    }
}
