using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using MCTGClassLibrary.DataObjects;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Users : IEndpointHandler
    {
        public Response HandleRequest(Request request)
        {
            if (request.Method.ToUpper() != "POST")
                return new Response("404", "Not Found");

            if (string.IsNullOrWhiteSpace(request.Payload))
                return new Response("400", "Bad Request");


            try
            {
                User user = JsonSerializer.Deserialize<User>(request.Payload);


                // database

                Console.WriteLine(user.Password);
                Console.WriteLine(user.Username);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return new Response("400", "Bad Request");
        }
    }
}
