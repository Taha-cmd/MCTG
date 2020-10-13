using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MCTGClassLibrary
{
    public class HTTPServer
    {
        private TcpListener listener;

        public bool Running { get; private set; }

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            listener.Start();
            Running = true;
        }

        public TcpClient AcceptClient()
        {
            TcpClient client = listener.AcceptTcpClient();
            return client;
        }

        public void HandleClient(TcpClient client)
        {
            try
            {
                Request request = new Request(client.GetStream());
                request.Display(ConsoleColor.Yellow);

                RequestHandler handler = new RequestHandler();
                Response response = handler.HandleRequest(request);

                response.AddHeader("Content-Type", "text");
                response.AddHeader("Server", "my shitty laptop");
                response.AddHeader("Date", DateTime.Today.ToString());

                response.Display(ConsoleColor.Green);

                response.Send(client.GetStream());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
