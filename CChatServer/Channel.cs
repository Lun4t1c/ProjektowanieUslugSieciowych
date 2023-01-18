using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CChatServer
{
    public class Channel
    {
        #region Properties
        public string Name { get; set; }
        public static Hashtable clientsList { get; set; } = new Hashtable();
        public int Port { get; set; }
        public bool IsOpen { get; set; } = false;
        public static List<string> StoredMessages { get; set; } = new List<string>();
        #endregion


        #region Constructor
        public Channel(string name)
        {
            Name = name;
        }
        #endregion


        #region Methods
        public async void Start(int port)
        {
            Port = port;

            TcpListener serverSocket = new TcpListener(IPAddress.Loopback, port);
            TcpClient clientSocket = default(TcpClient);

            serverSocket.Start();
            Console.WriteLine($"Channel '{Name}' is now listening on {Port}");

            while (true)
            {
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[100250];
                string dataFromClient = null;

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientsList.Add(dataFromClient, clientSocket);

                Broadcast("system", "system", dataFromClient + " has joined");

                Console.WriteLine(dataFromClient + " has joined chat room");
                ClientHandler client = new ClientHandler();
                client.startClient(clientSocket, dataFromClient, "user", clientsList);

                foreach (string payload in StoredMessages)
                    await Task.Run(() => _sendPayloadToUser(clientSocket, payload));
            }
        }

        public static void Broadcast(string user, string user_type, string message)
        {
            string broadcastString = CChatServer.ConstructPayload(user, user_type, message);

            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes = null;

                broadcastBytes = Encoding.ASCII.GetBytes(broadcastString);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }

        private async void _sendPayloadToUser(TcpClient tcpClient, string payload)
        {
            NetworkStream broadcastStream = tcpClient.GetStream();
            byte[] broadcastBytes = null;

            broadcastBytes = Encoding.ASCII.GetBytes(payload);

            broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
            broadcastStream.Flush();
        }
        #endregion
    }
}
