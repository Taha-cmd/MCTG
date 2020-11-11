using MCTGClassLibrary.Cards;
using MCTGClassLibrary.Database.Repositories;
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

                CardsRepository cardsRepository = new CardsRepository();
                int cardsAdded = cardsRepository.AddCards(cardDataArray);

                Console.WriteLine(cardsAdded);
                return new Response("200", "OK", $"{cardsAdded} cards added to the store");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error in Packages Handler: " + ex.Message);
            }

            return new Response();
        }
    }
}
