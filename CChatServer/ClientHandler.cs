using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CChatServer
{
    public class ClientHandler
    {
        TcpClient ClientSocket;
        string UserName;
        string UserType;
        Hashtable ClientsList;

        public void startClient(TcpClient inClientSocket, string userName, string userType, Hashtable cList)
        {
            this.ClientSocket = inClientSocket;
            this.UserName = userName;
            this.UserType = userType;
            this.ClientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        private void doChat()
        {
            int requestCount = 0;
            byte[] bytesFrom = new byte[100250];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;
            string rCount = null;
            requestCount = 0;

            Console.WriteLine(dataFromClient + " has joined server");
            while (true)
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = ClientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)ClientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    Console.WriteLine("From client - " + UserName + " : " + dataFromClient);
                    rCount = Convert.ToString(requestCount);

                    Console.WriteLine($"Broadcasting: ({UserName} : user : {dataFromClient})");
                    CChatServer.Broadcast(UserName, UserType, dataFromClient);
                    CChatServer.StoredMessages.Add(CChatServer.ConstructPayload(UserName, UserType, dataFromClient));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
