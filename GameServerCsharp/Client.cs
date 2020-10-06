using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace GameServerCsharp
{
    class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public Tcp tcp;

        public Client(int clientId)
        {
            id = clientId;
            tcp = new Tcp(id);
        }

        public class Tcp
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public Tcp(int id)
            {
                this.id = id;
            }

            public void Connect(TcpClient socket_)
            {
                socket = socket_;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                receiveBuffer = new byte[dataBufferSize];
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
            }

            private void ReceiveCallBack(IAsyncResult result)
            {
                try
                {
                    int byteLength = stream.EndRead(result);
                    if (byteLength <= 0)
                    {
                        return;
                    }

                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving Tcp data : {ex}");
                }
            }
        }
    }
}
