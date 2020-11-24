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

            //RequestHandler.RequestHandled += gameHandler.EventListener;
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



            /*  Player p1 = new Player("player1");
              Player p2 = new Player("player2");

              p1.MakeDeck(
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard()
                  );

              p2.MakeDeck(
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard(),
                  CardsManager.RandomCard()
                  );

            GameHandler.Instance.EnqueuePlayer(p1);
            GameHandler.Instance.EnqueuePlayer(p2);  */
        } 
    }
}