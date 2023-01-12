using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CChatServer
{
    public sealed class CChatServer
    {
        #region Config
        const int SERVER_PORT = 3243;
        #endregion

        #region Properties
        private static CChatServer _serverInstance = null;
        private TcpListener _serverSocket = new TcpListener(IPAddress.Loopback, SERVER_PORT);
        public static Hashtable clientsList = new Hashtable();
        #endregion


        #region Constructor
        private CChatServer()
        {            

        }
        #endregion


        #region Public Methods
        public static CChatServer GetServerInstance()
        {
            if (_serverInstance != null) 
                return _serverInstance;
            else
            {
                _serverInstance = new CChatServer();
                return _serverInstance;
            }
        }

        public void Start()
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Loopback, SERVER_PORT);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine($"Server is now listening on {SERVER_PORT}");
            counter = 0;

            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[100250];
                string dataFromClient = null;

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));

                clientsList.Add(dataFromClient, clientSocket);

                Broadcast(dataFromClient + " has joined ");

                Console.WriteLine(dataFromClient + " has joined chat room");
                ClientHandler client = new ClientHandler();
                client.startClient(clientSocket, dataFromClient, clientsList);
            }
        }

        public static void Broadcast(string message)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes = null;

                broadcastBytes = Encoding.ASCII.GetBytes(message);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }  //end broadcast function
        #endregion


        #region Private Methods

        #endregion
    }
}