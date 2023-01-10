using System;
using System.IO;
using CChatServer;

namespace CChatServerConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServerInstance serverInstance = ServerInstance.GetServerInstance();
            serverInstance.Start();
        }
    }
}