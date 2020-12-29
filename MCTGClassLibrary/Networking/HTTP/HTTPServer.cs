using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
            Console.WriteLine("Server Running...");
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

                Monitor.Enter(this);
                request.Display(ConsoleColor.Yellow);
                Monitor.Exit(this);

                RequestHandler handler = new RequestHandler();
                Response response = handler.HandleRequest(request);

                response.AddHeader("Content-Type", "text");
                response.AddHeader("Server", "my shitty laptop");
                response.AddHeader("Date", DateTime.Today.ToString());

                Monitor.Enter(this);
                response.Display(ConsoleColor.Green);
                Console.WriteLine("----------------------------------------------------------------------------------\n");
                Monitor.Exit(this);

                response.Send(client.GetStream());
                client.Close();
            }
            catch (Exception ex)
            {
                ex.Log();
            }

        }
    }
}
