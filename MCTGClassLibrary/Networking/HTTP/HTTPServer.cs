using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MCTGClassLibrary
{
    public class HTTPServer
    {
        public delegate void ClientHandler(Request req, NetworkStream clientStream);

        private TcpListener listener;
        private ClientHandler clientHandler;
        
        public HTTPServer(ClientHandler del)
        {
            clientHandler = del;
        }

        public void Start(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server Running...");

            while(true)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    var request = new Request(client.GetStream());
                    new Thread(() => clientHandler(request, client.GetStream())).Start();  
                }
                catch(Exception ex)
                {
                    ex.Log();
                }
            }
        }
    }
}
