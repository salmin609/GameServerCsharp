using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServerCsharp
{
    class Server
    {
        public static int MaxPlayer { get; private set; }
        public static int Port { get; private set; }

        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        private static TcpListener tcpListner;

        public static void Start(int maxPlayer, int port)
        {
            MaxPlayer = maxPlayer;
            Port = port;

            Console.WriteLine($"Starting server");
            InitializeServerData();

            tcpListner = new TcpListener(IPAddress.Any, Port);
            tcpListner.Start();
            tcpListner.BeginAcceptSocket(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Server start on {Port}");
        }

        private static void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient client = tcpListner.EndAcceptTcpClient(result);
            tcpListner.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            for (int i = 0; i < MaxPlayer; ++i)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }

            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect : Server full");
        }

        private static void InitializeServerData()
        {
            for (int i = 0; i < MaxPlayer; ++i)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
