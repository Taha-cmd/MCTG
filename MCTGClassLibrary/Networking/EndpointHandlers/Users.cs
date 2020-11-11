using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using MCTGClassLibrary.DataObjects;
using System.IO;
using MCTGClassLibrary.Database.Repositories;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Users : EndpointHandlerBase, IEndpointHandler
    {
        public Response HandleRequest(Request request)
        {
            return RouteToMethodHandler(request);
        }

        protected override Response PostHandler(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Payload))
                return new Response("No payload");

            try
            {
                UserData user;

                try
                {
                    user = JsonSerializer.Deserialize<UserData>(request.Payload);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Error in PostHandler in Users: " + ex.Message);
                    return new Response("Invalid Json Format");
                }

                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
                    return new Response("Username or Password Empty");

                UsersRepository users = new UsersRepository();

                if(users.UserExists(user.Username))
                    return new Response($"Username {user.Username} allready exists");

                if (users.RegisterUser(user.Username, user.Password))
                    return new Response("200", "OK", $"Account for {user.Username} created successfully");
   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Users: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
        }
    }
}
