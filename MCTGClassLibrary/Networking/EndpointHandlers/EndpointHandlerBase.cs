using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class EndpointHandlerBase
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

        protected Response RouteToMethodHandler(Request req)
        {
            switch(req.Method.ToUpper())
            {
                case "GET":         return GetHandler(req);
                case "PUT":         return PutHandler(req);
                case "POST":        return PostHandler(req);
                case "DELETE":      return DeleteHandler(req);
            }

            return new Response("405", "Method Not Allowed");
        }

        protected virtual Response GetHandler(Request request)      { return MethodNotSupported(); }
        protected virtual Response PostHandler(Request request)     { return MethodNotSupported(); }
        protected virtual Response PutHandler(Request request)      { return MethodNotSupported(); }
        protected virtual Response DeleteHandler(Request request)   { return MethodNotSupported(); }

        private Response MethodNotSupported()
        {
            return new Response("405", "Method Not Allowed", "this endpoint does not support this method");
        }
    }
}
