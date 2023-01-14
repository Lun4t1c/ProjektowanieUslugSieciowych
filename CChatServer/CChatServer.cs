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
        const string MESSAGE_SEPARATOR = ">|<";
        const char MESSAGE_SIGNOFF = '$';
        #endregion

        #region Properties
        private static CChatServer _serverInstance { get; set; } = null;
        //private TcpListener _serverSocket { get; set; } = new TcpListener(IPAddress.Loopback, SERVER_PORT);
        public static Hashtable clientsList { get; set; } = new Hashtable();
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

                Broadcast("system", "system", dataFromClient + " has joined");

                Console.WriteLine(dataFromClient + " has joined chat room");
                ClientHandler client = new ClientHandler();
                client.startClient(clientSocket, dataFromClient, "user", clientsList);
            }
        }

        public static void Broadcast(string user, string user_type, string message)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                byte[] broadcastBytes = null;

                string broadcastString = 
                    $"{user}{MESSAGE_SEPARATOR}" +
                    $"{user_type}{MESSAGE_SEPARATOR}" +
                    $"{message}{MESSAGE_SEPARATOR}" +
                    $"{MESSAGE_SIGNOFF}";

                broadcastBytes = Encoding.ASCII.GetBytes(broadcastString);

                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
        #endregion


        #region Private Methods

        #endregion
    }
}