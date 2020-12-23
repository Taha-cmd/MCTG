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



                Thread.CurrentThread.Name = username;
                var resp = ResponseManager.OK();


                // https://www.c-sharpcorner.com/UploadFile/1d42da/synchronization-events-and-wait-handles-in-C-Sharp/
                //https://docs.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?view=net-5.0

                GameHandler.Instance.BattleEnded += (object sender, BattleEndedEventArgs args) =>
                {
                    resp.AddPayload(args.BattleLog);
                    GameHandler.Instance.ResetEvent.Set(); // signal the thread to restart
                    
                };

                GameHandler.Instance.EnqueuePlayer(player);
                
                //Console.WriteLine($"Thread {Thread.CurrentThread.Name} handling player {username} is waiting for battle");
                GameHandler.Instance.ResetEvent.WaitOne(); // block thread untill it gets a signal
                //Console.WriteLine($"Thread {Thread.CurrentThread.Name} handling player {username} is relesed");

                // PROBLEM:
                /*
                    Thread A registers event listener, enqueues a player and waits for the battle in this specific order.
                    Thread B tries the same. BUT: when enqueuing for battle, the battle with take place thus call the callback from thread A,
                    which in turn will reset the ResetEvent and blocking the current thread.

                    needed solution: wait for both threads to end to reset the ResetEvent. how?
                    by waiting 1 sec, we ensure that both threads are released from the battle wait. and then we can reset the ResetEvent
                  
                 */


                // WARNING: not a perfect solution.
                // PROBLEM: if a new battle request is sent withen this one second (before the ResetEvent is reset),
                // the handling thread will not wait for the battle, since the ResetEvent is not yet reset
                Thread resetEventReseter = new Thread(() =>
                {
                    Thread.Sleep(1000); // wait 1 sec
                    GameHandler.Instance.ResetEvent.Reset();
                });

                resetEventReseter.Start();
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
