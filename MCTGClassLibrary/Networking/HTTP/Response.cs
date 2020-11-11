using MCTGClassLibrary.Database.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;

namespace MCTGClassLibrary
{
    public class Response : ResponseRequestBase
    {

        private string payload = "";
        
        public string Protocol { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public Response(string status = "400", string message = "Bad Request", string payload = "")
        {
            Protocol = Config.PROTOCOL;
            Status = status;
            StatusMessage = message;

            AddPayload(payload);
            Values = new Dictionary<string, string>();
        }

        public Response(string payload)
        {
            Protocol = Config.PROTOCOL;
            Status = "400";
            StatusMessage = "Bad Request";


            Values = new Dictionary<string, string>();
            AddPayload(payload);
        }

        public void AddHeader(string key, string value)
        {
            if (Values.ContainsKey(key))
                Values[key] = value;
            else
                Values.Add(key, value);
        }

        public void AddPayload(string payload)
        {
            this.payload += payload;
        }

        public override void Display(ConsoleColor color)
        {
            PrintInColor("Protocol", Protocol, color);
            PrintInColor("Status", Status, color);
            PrintInColor("Status Message", StatusMessage, color);
            base.Display(color);
        }

        public void Send(NetworkStream client)
        {
            // using statement auto disposes and flushes the stream
            using(StreamWriter writer = new StreamWriter(client))
            {
                writer.Write($"{Protocol} {Status} {StatusMessage}\r\n");

                foreach (var kvp in Values)
                    writer.Write($"{kvp.Key}: {kvp.Value}\r\n");

                writer.Write($"Content-Length: {payload.Length}\r\n");
                writer.Write("\r\n");
                writer.Write(payload);
                writer.Write("\r\n\r\n");
            }
        }
    }
}
