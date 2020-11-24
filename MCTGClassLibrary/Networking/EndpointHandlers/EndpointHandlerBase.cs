using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerBase : IEndpointHandler
    {
        protected EndpointHandlerBase() { }

        protected bool Authorized(string authorizationString)
        {
            string username = ExtractUserNameFromAuthoriazionHeader(authorizationString);
            return new UsersRepository().UserExists(username);
        }

        protected string ExtractUserNameFromAuthoriazionHeader(string authorizationString)
        {
            string username;

            try
            {
                username = authorizationString.Substring(authorizationString.IndexOf(" ") + 1);
                username = username.Substring(0, username.IndexOf("-"));
            }
            catch(Exception ex)
            {
                throw new InvalidDataException("Error extracting name from authorization header. Invalid format");
            }

            return username;
        }

        protected string GetNthTokenFromRoute(int index, string route)
        {
            string[] tokens = route.Split("/");

            if (index >= tokens.Length)
                throw new InvalidDataException("Invalid format");

            return tokens[index];
        }

        public Response HandleRequest(Request request)
        {
            switch (request.Method.ToUpper())
            {
                case "GET":     return GetHandler(request);
                case "PUT":     return PutHandler(request);
                case "POST":    return PostHandler(request);
                case "DELETE":  return DeleteHandler(request);
            }

            return new Response("405", "Method Not Allowed", $"The method {request.Method.ToUpper()} is not supported");
        }

        protected virtual Response GetHandler(Request request)      { return MethodNotSupported("GET"); }
        protected virtual Response PostHandler(Request request)     { return MethodNotSupported("POST"); }
        protected virtual Response PutHandler(Request request)      { return MethodNotSupported("PUT"); }
        protected virtual Response DeleteHandler(Request request)   { return MethodNotSupported("DELETE"); }

        private Response MethodNotSupported(string method)
        {
            return new Response("405", "Method Not Allowed", $"this endpoint does not support the method {method}");
        }
    }
}
