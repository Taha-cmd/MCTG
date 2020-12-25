using MCTGClassLibrary.Database.Repositories;
using MCTGClassLibrary.Game;
using MCTGClassLibrary.Networking.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MCTGClassLibrary.Networking.EndpointHandlers
{
    public class Battles : EndpointHandlerBase
    {
        // post to battles => requst for a battle
        protected override Response PostHandler(Request request)
        {
            if (request.Authorization.IsNullOrWhiteSpace())
                return ResponseManager.BadRequest("Authoriazion Header required");

            if (!Authorized(request.Authorization))
                return ResponseManager.Unauthorized("This action requires authorization");

            try
            {
                string username = ExtractUserNameFromAuthoriazionHeader(request.Authorization);
                Player player = new Player(username);

                var decksRepo = new DecksRepository();

                if (decksRepo.Empty(username))
                    return ResponseManager.BadRequest($"The Deck for user {username} is empty, Configure your deck before entering a battle");

                var deck = decksRepo.GetDeck(username);
                player.MakeDeck(deck);

                if (GameHandler.Instance.Enqueued(username))
                    return ResponseManager.BadRequest($"Player {username} is allready enqueued for a battle! You can't fight yourself");

                var resp = ResponseManager.Created();

                // https://www.c-sharpcorner.com/UploadFile/1d42da/synchronization-events-and-wait-handles-in-C-Sharp/
                //https://docs.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?view=net-5.0

                GameHandler.Instance.BattleEnded += (object sender, BattleEndedEventArgs args) =>
                {
                    resp.AddPayload(args.BattleLog);
                    GameHandler.Instance.ResetEvent.Set();      // send the signal, all waiting threads will start running (open the door)
                    GameHandler.Instance.ResetEvent.Reset();    // reset the flag, new threads can wait (close the door)
                    
                };

                GameHandler.Instance.EnqueuePlayer(player);
                GameHandler.Instance.ResetEvent.WaitOne(); // block thread untill it gets a signal

                return resp;
            }
            catch (InvalidDataException ex)
            {
                return ResponseManager.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Battles PostHandler: " + ex.Message);
            }

            GameHandler.Instance.ResetEvent.Reset();
            return ResponseManager.InternalServerError("Internal Server Error");
        }
    }
}
