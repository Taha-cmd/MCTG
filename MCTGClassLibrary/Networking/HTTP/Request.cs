﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace MCTGClassLibrary
{
    public class Request : ResponseRequestBase
    {
        public string RequestString { get; private set; }
        public Dictionary<string, string> QueryParams { get; private set; }
        public string Method { get; private set; }
        public string Endpoint { get; private set; }
        public string Payload { get; private set; }
        public string Authorization { get; private set; }
        public string Route { get; private set; }
        public string[] RouteTokens { get; private set; }

        public Request(NetworkStream clientStream)
        {
            StreamReader reader = new StreamReader(clientStream);

            // read single chars
            // readLine() will block at the empty line
            while (reader.Peek() >= 0)
                RequestString += (char)reader.Read();

            Values = new Dictionary<string, string>();
            QueryParams = new Dictionary<string, string>();

            ParseRequest();
            ParseQueryParams();
            ParseRouteTokens();
        }
        public override void Display(ConsoleColor color = ConsoleColor.Green)
        {
            base.Display(color);

            Console.WriteLine("Query Parameters:");
            foreach (var kvp in QueryParams)
                PrintInColor(kvp.Key, kvp.Value, ConsoleColor.Magenta);
            Console.WriteLine("\n");
        }


        private void ParseRequest()
        {
            if (RequestString.IsNullOrWhiteSpace())
                throw new InvalidDataException();

            string[] lines = RequestString.Split("\r\n");

            // first line has format METHOD ROUTE PROTOCOL
            string[] tokens = lines[0].Split(' ');

            if (tokens.Length != 3)
                throw new InvalidDataException();

            Values.Add("Method", tokens[0]);
            Values.Add("Route", tokens[1]);
            Values.Add("Protocol", tokens[2]);

            // extract endpoint from Route
            string[] endpointTokens = Values["Route"].Split('/');
            string endpoint = !endpointTokens[1].IsNullOrWhiteSpace() ? endpointTokens[1] : "home";

            // trim out query parameters
            endpoint = endpoint.Contains('?') ? endpoint.Substring(0, endpoint.IndexOf('?')) : endpoint;
            endpoint = endpoint.IsNullOrWhiteSpace() ? "home" : endpoint;

            Values.Add("Endpoint", endpoint);

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
                else if( lines[i].IsNullOrWhiteSpace()  )
                {
                    Values.Add("Payload", "");
                    for(int j = i + 1; j < lines.Length; j++)
                        Values["Payload"] += lines[j] + "\n";

                    break;
                }
            }

            Method          =   Values["Method"];
            Endpoint        =   Values["Endpoint"];
            Payload         =   Values["Payload"];
            Authorization   =   Values.ContainsKey("Authorization") ? Values["Authorization"] : null;
            Route           =   Values["Route"];
        }

        private void ParseQueryParams()
        {
            if (!Values["Route"].Contains('?'))
                return;

            // SkipWhile not skipping last element? use where
            QueryParams =    Values["Route"].Split("?")
                            .Where(el => el.Contains("="))
                            .ToDictionary
                             (
                                el => el.Substring(0, el.IndexOf("=")).Trim(), // key selector
                                el => el.Substring(el.IndexOf("=") + 1).Trim() // value selector
                             );
        }
        private void ParseRouteTokens()
        {
            string route = Route.Contains("?") ? Route.Substring(0, Route.IndexOf("?")) : Route;
            RouteTokens =   route.Split("/")
                            .SkipWhile(el => el.IsNullOrWhiteSpace())
                            .ToArray();

            Values.Add("RouteTokens", RouteTokens.Serialize());
        }
    }
}
