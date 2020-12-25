using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;
using System.Linq;
using MCTGClassLibrary.Game;
using System.IO;
using MCTGClassLibrary.Database.Repositories;

namespace MCTGClassLibrary
{
    // Singleton Design Pattern
    // static initialzation is threadsafe
    // static constructor will be called by the clr once and init an object
    // this object can be accessed by a property
    // constructor is private to prevent the creation of other instances

    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-constructors
    // https://www.c-sharpcorner.com/UploadFile/8911c4/singleton-design-pattern-in-C-Sharp/#:~:text=Thread%20Safety%20Singleton&text=This%20implementation%20is%20thread%2Dsafe,thread%20will%20create%20an%20instance.
    public class GameHandler
    {
        private static readonly GameHandler instance; // class holds reference to the single object
        static GameHandler() // static constructor will be called once by the clr, construct the instance
        {
            instance = new GameHandler();
        }

        public static GameHandler Instance { get { return instance; } } // access point

        private Queue<Player> queue;
        private GameHandler()
        {
            queue = new Queue<Player>();
        }

        public int PlayersInQueueCount() => queue.Count;
        public string[] EnqueuedPlayers() => queue.Select(player => player.Name).ToArray();
        public bool Enqueued(string username) => EnqueuedPlayers().Contains(username);

        public void EnqueuePlayer(Player player)
        {
            if (!player.HasDeck)
                throw new InvalidDataException("Only players with a deck can be enqueued!");

            queue.Enqueue(player);

            Monitor.Enter(this);

            if (queue.Count == 2)
                new Thread(() => StartBattle(queue.Dequeue(), queue.Dequeue())).Start();
            
            Monitor.Exit(this);
        }

        // https://www.c-sharpcorner.com/UploadFile/1d42da/synchronization-events-and-wait-handles-in-C-Sharp/
        // https://docs.microsoft.com/en-us/dotnet/api/system.threading.manualresetevent?view=net-5.0
        public ManualResetEvent ResetEvent { get; private set; } = new ManualResetEvent(false);

        private void StartBattle(Player player1, Player player2)
        {
            var random = new Random();
            bool draw = true;
            string? winner = null;
            string? loser = null;

            // at least 4 rounds, each round describe both cards
            var stringBuilder = new StringBuilder(player1.Deck.GetRandomCard().Description().Length * 10);

            int attackerIndex = random.Next(0, 2);
            int defenderIndex = attackerIndex == 0 ? 1 : 0;

            Deck[] decks = { player1.Deck, player2.Deck };

            int i = 0;
            for(; i < 100; i++)
            {
                Card attacker = decks[attackerIndex].GetRandomCard();
                Card defender = decks[defenderIndex].GetRandomCard();


                stringBuilder.AppendLine($"\t---round {i+1}---\t");
                stringBuilder.AppendLine($"attacker: {decks[attackerIndex].Owner}, (attacking card)\n{attacker.Description()}\n vs \n\ndefender: " +
                    $"{decks[defenderIndex].Owner} (defending card)\n{defender.Description()}");
                

                if(attacker.Attack(defender))
                {
                    decks[attackerIndex].Extend(defender);
                    decks[defenderIndex].Remove(defender);
                    stringBuilder.AppendLine($"player {decks[attackerIndex].Owner} with card {attacker.Name} wins round {i+1}");
                }
                else
                {
                    decks[defenderIndex].Extend(attacker);
                    decks[attackerIndex].Remove(attacker);
                    stringBuilder.AppendLine($"player {decks[defenderIndex].Owner} with card {defender.Name} wins round {i + 1}");
                }

                if(decks[attackerIndex].Empty || decks[defenderIndex].Empty)
                {
                    draw = false;
                    break;
                }

                stringBuilder.AppendLine("-------------------------------------------------------------\n");
                Swap(ref attackerIndex, ref defenderIndex);
            }

            stringBuilder.AppendLine("\n*******************************************************************\n");
            stringBuilder.AppendLine("GAME OVER");
            stringBuilder.AppendLine($"Played rounds: {i + 1}");

            if(!draw)
            {
                if (!decks[attackerIndex].Empty)
                {
                    stringBuilder.AppendLine($"{decks[attackerIndex].Owner} won the game with {decks[attackerIndex].Count} cards in the deck");
                    winner = decks[attackerIndex].Owner;
                    loser = decks[defenderIndex].Owner;
                } 
                else
                {
                    stringBuilder.AppendLine($"{decks[defenderIndex].Owner} won the game with {decks[defenderIndex].Count} cards in the deck");
                    winner = decks[defenderIndex].Owner;
                    loser = decks[attackerIndex].Owner;
                }
                    
            }
            else
            {
                stringBuilder.AppendLine("DRAW");
            }

            OnBattleEnded(new BattleEndedEventArgs(stringBuilder.ToString()));

            var battlesRepo = new BattlesRepository();
            battlesRepo.AddBattle(decks[attackerIndex].Owner, decks[defenderIndex].Owner, winner, stringBuilder.ToString(), i+1);

            if(!draw)
            {
                var scoreBoard = new ScoresRepository();
                scoreBoard.IncreaseBattles(winner);
                scoreBoard.IncreaseBattles(loser);

                scoreBoard.WonBattle(winner);
                scoreBoard.LostBattle(loser);

                scoreBoard.IncreaseScore(winner, Config.POINTSPERWIN);
                scoreBoard.IncreaseScore(loser, Config.POINTSPERLOSE);
            }
            
        }

        private void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        public event EventHandler<BattleEndedEventArgs> BattleEnded;
        protected void OnBattleEnded(BattleEndedEventArgs args)
        {
            BattleEnded?.Invoke(this, args);
            //BattleEnded = null; // clear event handlers after firing it
        }

        /*public void EventListener(object publisher, RequestEventArgs args)
        {
            Console.WriteLine(args.Test);
        } */

    }
}
