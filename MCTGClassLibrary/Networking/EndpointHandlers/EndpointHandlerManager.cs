using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerManager
    {
        private EndpointHandlerManager() { }
        public static IEndpointHandler Get(string endpoint)
        {
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
