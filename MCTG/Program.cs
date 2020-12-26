using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MCTGClassLibrary;
using MCTGClassLibrary.Cards;
using MCTGClassLibrary.Database;
using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.Enums;
using System.Runtime.Serialization;
using System.Text.Json;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;

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
                catch(Exception ex)
                {
                    ex.Log();
                }
            }  
        } 
    }
}