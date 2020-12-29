using MCTGClassLibrary;
using MCTGClassLibrary.Database.Repositories;
using System;
using System.Net.Sockets;
using System.Threading;

namespace MCTG
{
    class Program
    {
        static void Main(string[] args)
        {

            HTTPServer server = new HTTPServer(Config.LISTENINGPORT);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptClient();

                try
                {
                    new Thread(() => server.HandleClient(client)).Start();
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }
        }
    }
}