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
        // GET to /deck => show current deck
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

        // PUT to /deck => configure deck
        protected override Response PutHandler(Request request)
        {
            if(request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (request.Payload.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("No Payload");

            try
            {
                if (!Authorized(request.Authorization))
                    return ResponseManager.Unauthorized("check your username and password");

                string username = ExtractUserNameFromAuthoriazionHeader(request.Authorization);
                var cards = new List<string>();
                
                try
                {
                    cards = JsonSerializer.Deserialize<List<string>>(request.Payload);
                }
                catch(Exception ex)
                {
                    return ResponseManager.BadRequest("Invalid Json Format");
                }

                // easy solution - accept only four cards. what if user wants to change only one card?
                // consider making this dynamic
                if (cards.Count != Config.DECKSIZE)
                    return ResponseManager.BadRequest($"Deck size must be {Config.DECKSIZE}");

                var decks = new DecksRepository();
                decks.UpdateDeck(username, cards.ToArray());  // TEST THIS 

                return ResponseManager.OK("deck updated successfully");
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Deck PutHandler: " + ex.Message);
            }

            return ResponseManager.InternalServerError();
        }
    }
}
