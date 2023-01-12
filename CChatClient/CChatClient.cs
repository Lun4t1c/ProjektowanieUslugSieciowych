using System.Net;
using System.Net.Sockets;

namespace CChat
{
    public class CChatClient
    {
        #region Properties
        public bool IsActive { get; set; } = false;
        TcpClient clientSocket;
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public delegate void PassMessageDelegate(string message);
        PassMessageDelegate PassMessage;
        #endregion


        #region Constructor
        public CChatClient()
        {
            
        }
        #endregion


        #region Methods
        public void Initialize(string userName, PassMessageDelegate passMessageDelegate = null)
        {
            PassMessage = passMessageDelegate;

            IPAddress address = IPAddress.Loopback;
            int port = 3243;

            clientSocket = new TcpClient();
            clientSocket.Connect(address, port);

            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(userName + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void Start()
        {
            PassMessage("Starting client...");
            IsActive = true;
            while (true)
            {
                if (!IsActive) break;

                serverStream = clientSocket.GetStream();

                int buffSize = 0;
                byte[] inStream = new byte[100250];
                buffSize = clientSocket.ReceiveBufferSize;
                serverStream.Read(inStream, 0, buffSize);
                string returndata = System.Text.Encoding.ASCII.GetString(inStream);
                readData = "" + returndata;

                if (!string.IsNullOrEmpty(readData))
                {
                    Console.WriteLine($">> {readData}");
                    if (PassMessage != null) PassMessage(readData);
                }
            }
        }

        public void Stop()
        {
            IsActive = false;
        }

        public void SendMessage(string message)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(message + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        #endregion
    }
}