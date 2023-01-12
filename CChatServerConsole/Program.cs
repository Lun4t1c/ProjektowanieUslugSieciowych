using System;
using System.IO;
using CChatServer;

namespace CChatServerConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CChatServer.CChatServer serverInstance = CChatServer.CChatServer.GetServerInstance();
            serverInstance.Start();
        }
    }
}