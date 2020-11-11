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
    public class Packages : EndpointHandlerBase,  IEndpointHandler
    {
        public Response HandleRequest(Request request)
        {
            return RouteToMethodHandler(request);
        }

        protected override Response PostHandler(Request request)
        {
            if (string.IsNullOrWhiteSpace(request.Payload) || string.IsNullOrEmpty(request.Payload))
                return new Response("No Payload");

            try
            {
                CardData[] cardDataArray;

                try
                {
                    cardDataArray = JsonSerializer.Deserialize<CardData[]>(request.Payload);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in PostHandler in Packages: " + ex.Message);
                    return new Response("Invalid Json Format");
                }

                CardsRepository cardsRepository = new CardsRepository();
                int cardsAdded = cardsRepository.AddCards(cardDataArray);

                return new Response("200", "OK", $"{cardsAdded} cards added to the store");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in PostHandler in Packages: " + ex.Message);
            }

            return new Response("500", "Internal Server Error");
        }

    }
}
