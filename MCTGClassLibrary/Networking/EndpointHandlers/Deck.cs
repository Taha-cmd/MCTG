using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Networking.HTTP;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Deck : EndpointHandlerBase
    {
        // GET to /Deck => show current deck
        protected override Response GetHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            try
            {
                if (!Authorized(request.Authorization))
                    return ResponseManager.Unauthorized("check your username and password");

                string username = ExtractUserNameFromAuthoriazionHeader(request.Authorization);

                var deckRepo = new DecksRepository();
                if (deckRepo.Empty(username))
                    return ResponseManager.OK($"the deck for user {username} is empty");

                CardData[] cards  = deckRepo.GetDeck(username);
                string deck = JsonSerializer.Serialize<CardData[]>(cards);

                return ResponseManager.OK(deck);
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Deck GetHandler: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }
    }
}
