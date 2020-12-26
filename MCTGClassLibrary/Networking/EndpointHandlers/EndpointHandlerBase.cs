using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MCTGClassLibrary.Networking.HTTP;
using System.Text.Json;

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
            catch(Exception)
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
            try
            {
                switch (request.Method.ToUpper())
                {
                    case "GET":     return GetHandler(request);
                    case "PUT":     return PutHandler(request);
                    case "POST":    return PostHandler(request);
                    case "DELETE":  return DeleteHandler(request);
                    default:        return ResponseManager.MethodNotAllowed($"The method {request.Method.ToUpper()} is not supported");
                }
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch(JsonException ex)
            {
                return ResponseManager.BadRequest("Invalid json: " + ex.Message);
            }
            catch(Exception ex)
            {
                ex.Log();
            }

            return ResponseManager.InternalServerError(); 
        }

        protected virtual Response GetHandler(Request request)      => MethodNotImplemented("GET");
        protected virtual Response PostHandler(Request request)     => MethodNotImplemented("POST");
        protected virtual Response PutHandler(Request request)      => MethodNotImplemented("PUT");
        protected virtual Response DeleteHandler(Request request)   => MethodNotImplemented("DELETE");

        private Response MethodNotImplemented(string method) => ResponseManager.NotImplemented($"this endpoint hasn't implemented {method} yet");
    }
}
