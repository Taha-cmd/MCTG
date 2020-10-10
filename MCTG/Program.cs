using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MCTGClassLibrary;

namespace MCTG
{
    class Program
    {
        static void Main(string[] args)
        {
            HTTPServer server = new HTTPServer(8080);
            GameHandler gameHandler = new GameHandler();

            RequestHandler.RequestHandled += gameHandler.EventListener;
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptClient();
                Thread requestHandlingThread;

                try
                {
                    requestHandlingThread = new Thread(() => server.HandleClient(client));
                    requestHandlingThread.Start();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
   
            }
        }
    }
}