using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using CChat;

namespace CChatClientConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Name > ");
            string name = Console.ReadLine();

            CChatClient client = new CChatClient();

            client.Initialize(name, CatchMessage);
            client.Start();
        }

        public static void CatchMessage(string message)
        {
            Console.WriteLine($"CATCHED: {message}");
        }
    }
}