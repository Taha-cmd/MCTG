using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
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
            string username = authorizationString.Substring(authorizationString.IndexOf(" ") + 1);
            username = username.Substring(0, username.IndexOf("-"));

            return username;
        }

        public Response HandleRequest(Request request)
        {
            return RouteToMethodHandler(request);
        }

        protected Response RouteToMethodHandler(Request req)
        {
            switch(req.Method.ToUpper())
            {
                case "GET":         return GetHandler(req);
                case "PUT":         return PutHandler(req);
                case "POST":        return PostHandler(req);
                case "DELETE":      return DeleteHandler(req);
            }

            return new Response("405", "Method Not Allowed", $"The method {req.Method.ToUpper()} is not supported");
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
