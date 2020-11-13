using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading;

namespace MCTGClassLibrary
{
    // Singleton Design Pattern
    // static initialzation is threadsafe
    // static constructor will be called by the clr once and init an object
    // this object can be accessed by a property
    // constructor is private

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

        public void EnqueuePlayer(Player player)
        {
            if (!player.HasDeck)
                throw new DataException("Only players with a deck can be enqueued!");

            queue.Enqueue(player);

            Monitor.Enter(this);

            if (queue.Count == 2)
                StartBattle(queue.Dequeue(), queue.Dequeue());

            Monitor.Exit(this);
        }

        private void StartBattle(Player player1, Player player2)
        {
            var random = new Random();
            bool draw = true;

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
                stringBuilder.AppendLine($"(attacking card)\n{attacker.Description()}\n vs \n\n(defending card)\n{defender.Description()}");
                

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
                    stringBuilder.AppendLine($"{decks[attackerIndex].Owner} won the game with {decks[attackerIndex].Count} cards in the deck");
                else
                    stringBuilder.AppendLine($"{decks[defenderIndex].Owner} won the game with {decks[defenderIndex].Count} cards in the deck");
            }
            else
            {
                stringBuilder.AppendLine("DRAW");
            }

            Console.WriteLine(stringBuilder.ToString());
        }

        private void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        /*public void EventListener(object publisher, RequestEventArgs args)
        {
            Console.WriteLine(args.Test);
        } */

    }
}
