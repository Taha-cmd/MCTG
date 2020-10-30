using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerManager
    {
        private EndpointHandlerManager() { }

        private static string[] endpoints = { 
            "home", "users", "sessions", "packages", 
            "battles", "cards", "deck", "score",
            "stats", "tradings", "transactions"
        };

        public static IEndpointHandler Get(string endpoint)
        {
            if (!Array.Exists(endpoints, element => element == endpoint))
                return null;

            switch (endpoint.ToLower())
            {
                case "users":           return new Users();
                case "packages":        return new Packages();
                case "sessions":        return new Sessions();
                case "battles":         return new Battles();
                case "cards":           return new Cards();
                case "deck":            return new Deck();
                case "score":           return new Score();
                case "stats":           return new Stats();
                case "tradings":        return new Tradings();
                case "transactions":    return new Transactions();
                case "home":            return new Home();
            }

            return null;
        }
    }
}
