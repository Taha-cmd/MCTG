using MCTGClassLibrary;
using MCTGClassLibrary.Database.Repositories;
using System;
using System.Net.Sockets;

namespace MCTG
{
    class Program
    {
        private static object dummyobject = new object();
        static void Main(string[] args)
        {

            HTTPServer server = new HTTPServer((Request request, NetworkStream clientStream) =>
            {
                lock(dummyobject)
                {
                    request.Display(ConsoleColor.Yellow);
                }

                RequestHandler handler = new RequestHandler();
                Response response = handler.HandleRequest(request);

                response.AddHeader("Content-Type", "text");
                response.AddHeader("Server", "my shitty laptop");
                response.AddHeader("Date", DateTime.Today.ToString());

                lock(dummyobject)
                {
                    response.Display(ConsoleColor.Green);
                    Console.WriteLine("----------------------------------------------------------------------------------\n");
                }

                response.Send(clientStream);
            });

            server.Start(Config.LISTENINGPORT);
        }
    }
}