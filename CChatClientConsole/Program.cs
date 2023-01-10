using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CChatClientConsole
{
    public class Program
    {
        static TcpClient clientSocket;
        static NetworkStream serverStream = default(NetworkStream);
        static string readData = null;

        public static void Main(string[] args)
        {
            Initialize();
            //Loop();
        }

        public static void Initialize()
        {
            IPAddress address = IPAddress.Loopback;
            int port = 3243;

            clientSocket = new TcpClient();

            Console.Write("Connecting to server... ");
            clientSocket.Connect(address, port);
            Console.Write("Name: ");
            string name = Console.ReadLine();

            serverStream = clientSocket.GetStream();

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(name + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public static void Loop()
        {
            serverStream = clientSocket.GetStream();

            int buffSize = 0;
            byte[] inStream = new byte[100250];
            buffSize = clientSocket.ReceiveBufferSize;
            serverStream.Read(inStream, 0, buffSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            readData = "" + returndata;

            return;
        }
    }
}