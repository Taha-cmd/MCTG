using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Cards : EndpointHandlerBase
    {

        // should return all cards of a user (the stack)
        protected override Response GetHandler(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Authorization))
                return new Response("No Authorization Header found");

            if (!Authorized(request.Authorization))
                return new Response("Authorization Failed!, check your username and password");

            CardData[] cardDataArray = new UsersRepository().GetStack( ExtractUserNameFromAuthoriazionHeader(request.Authorization) );

            if (cardDataArray.Length == 0)
                return new Response("Stack is empty");

            try
            {
                string data = JsonSerializer.Serialize<CardData[]>(cardDataArray);
                return new Response("200", "OK", data);
            }
            catch(InvalidDataException ex)
            {
                return new Response(ex.Message);
            }
            catch(Exception x)
            {
                Console.WriteLine("error in GetHandler in Cards: " + x.Message);
            }

            return new Response("500", "Internal Server Error");
        }
    }
}
