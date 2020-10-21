using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerManager
    {
        private EndpointHandlerManager() { }
        private static string[] endpoints = { "users", "sessions", "packages", "Home" };

        public static IEndpointHandler Get(string endpoint)
        {
                if (!Array.Exists(endpoints, element => element == endpoint))
                    return null;

                switch (endpoint.ToLower())
                {
                    case "users": return new Users();
                    case "packages": return new Packages();
                    case "sessions": return new Sessions();
                }

                return null;

        }

    }
}
