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
            Request request = new Request(client.GetStream());
            request.Display();

            Response response = RequestHandler.HandleRequest(request);
            response.Send(client.GetStream());
        }
    }
}
