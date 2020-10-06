using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerCsharp
{
    class ServerHandle
    {
        public static void WelcomeReceived(int fromClient, Packet packet)
        {
            int clientId = packet.ReadInt();
            string userName = packet.ReadString();

            Console.WriteLine($"{Server.clients[fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully " +
                              $"and now is player {fromClient}");

            if (fromClient != clientId)
            {
                Console.WriteLine($"Player : {userName} has wrong client Id");
            }
        }

    }
}
