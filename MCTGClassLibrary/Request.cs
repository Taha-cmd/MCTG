﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MCTGClassLibrary
{
    public class Request
    {
        private string request;
        public Dictionary<string, string> Values { get; private set; }
        public Request(NetworkStream clientStream)
        {
            StreamReader reader = new StreamReader(clientStream);

            while (reader.Peek() != -1)
                request += reader.ReadLine() + "\r\n";

            Values = new Dictionary<string, string>();
            ParseRequest();
        }

        public void Display()
        {
            Console.Write('\n');

            foreach (KeyValuePair<string, string> kvp in Values)
                PrintInColor(kvp.Key, kvp.Value);
            
            Console.Write('\n');
        }

        private void PrintInColor(string key, string value, ConsoleColor color = ConsoleColor.Green)
        {
            Console.Write(key + ": ");
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ParseRequest()
        {
            string[] lines = request.Split("\r\n");

            // first line has format METHOD ROUTE PROTOCOL
            string[] tokens = lines[0].Split(' ');
            Values.Add("Method", tokens[0]);
            Values.Add("Route", tokens[1]);
            Values.Add("Protocol", tokens[2]);

            // rest of lines has format key: value
            // after the empty line comes the body
            for(int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Contains(':'))
                {
                    int splitIndex = lines[i].IndexOf(':');
                    string key = lines[i].Substring(0, splitIndex);
                    string value = lines[i].Substring(splitIndex + 1).Trim();
                    Values.Add(key, value);
                }
                else if( string.IsNullOrWhiteSpace(lines[i])  )
                {
                    Values.Add("Payload", "");
                    for(int j = i + 1; j < lines.Length; j++)
                        Values["Payload"] += lines[j] + "\n";

                    break;
                }
            }
        }
    }
}
