using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CChat
{
    public class ChannelWindow
    {
        #region Properties
        public string Name { get; set; }
        public int Port { get; set; }
        public bool IsOpen { get; set; } = false;

        TcpClient clientSocket;
        NetworkStream serverStream = default(NetworkStream);
        string readData = null;

        public delegate void PassMessageDelegate(string message);
        PassMessageDelegate PassMessage;
        #endregion


        #region Constructor
        public ChannelWindow(string name, int port)
        {
            Name = name;
            Port = port;
        }
        #endregion


        #region Methods

        public void Start()
        {
            PassMessage(
                $"system{CChatClient.MESSAGE_SEPARATOR}" +
                $"system{CChatClient.MESSAGE_SEPARATOR}" +
                $"Starting client...{CChatClient.MESSAGE_SEPARATOR}" +
                $"{CChatClient.MESSAGE_SIGNOFF}"
            );

            IsOpen = true;
            while (IsOpen)
            {
                if (!IsOpen) break;

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
        #endregion
    }
}
