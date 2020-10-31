using MCTGClassLibrary.Cards;
using MCTGClassLibrary.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Packages : IEndpointHandler
    {
        public Response HandleRequest(Request request)
        {
            if (request.Method.ToUpper() != "POST")
                return new Response("404", "Not Found");

            if (string.IsNullOrWhiteSpace(request.Payload) || string.IsNullOrEmpty(request.Payload))
                return new Response("404", "Not Found");

            try
            {

                CardData[] cardDataArray = JsonSerializer.Deserialize<CardData[]>(request.Payload);
                Card[] cards = cardDataArray.Select(cd => CardsManager.Create(cd)).ToArray();

                foreach(var cd in cardDataArray)
                    Console.WriteLine($"{cd.Id}, {cd.Name}, {cd.Damage}, {cd.Weakness}");

                foreach (var cd in cards)
                    Console.WriteLine(cd.Description());

                Console.WriteLine();

                // database

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }




            return new Response();
        }
    }
}
