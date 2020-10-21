using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MCTGClassLibrary;
using MCTGClassLibrary.Cards;

namespace MCTG
{
    class Program
    {
        static void Main(string[] args)
        {
            HTTPServer server = new HTTPServer(10001);
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





            /*  Player p1 = new Player("player1");
              Player p2 = new Player("player2");

              p1.makeDeck(
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard()
                  );

              p2.makeDeck(
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard(),
                  CardGenerator.RandomCard()
                  );

              GameHandler handler = new GameHandler();
              handler.EnqueuePlayer(p1);
              handler.EnqueuePlayer(p2);  */
        } 
    }
}