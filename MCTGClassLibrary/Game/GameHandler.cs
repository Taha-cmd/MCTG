using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MCTGClassLibrary
{
    public class GameHandler //: IRequestHandler
    {
        public GameHandler() { }

        private Queue< Player > queue = new Queue< Player >();

        public void EnqueuePlayer(Player player)
        {
            if (!player.HasDeck)
                throw new DataException("Only players with a deck can be enqueued!");

            queue.Enqueue(player);

            if (queue.Count == 2)
                StartBattle(queue.Dequeue(), queue.Dequeue());
        }

        private void StartBattle(Player player1, Player player2)
        {
            var random = new Random();

            int attackerIndex = random.Next(0, 2);
            int defenderIndex = attackerIndex == 0 ? 1 : 0;

            Deck[] decks = { player1.Deck, player2.Deck };

            for(int i = 0; i < 100; i++)
            {
                Card attacker = decks[attackerIndex].GetRandomCard();
                Card defender = decks[defenderIndex].GetRandomCard();


                Console.WriteLine($"\t---round {i+1}---\t");
                Console.WriteLine(attacker.Describe() + " vs " + defender.Describe());
                

                if(attacker.Attack(defender))
                {
                    decks[attackerIndex].Extend(defender);
                    decks[defenderIndex].Remove(defender);
                    Console.WriteLine("attacker " + attacker.Describe() + " wins");
                }
                else
                {
                    decks[defenderIndex].Extend(attacker);
                    decks[attackerIndex].Remove(attacker);
                    Console.WriteLine("defender " + defender.Describe() + " wins");
                }

                if(decks[attackerIndex].Empty || decks[defenderIndex].Empty)
                {
                    Console.WriteLine("GAME OVER");

                    if(!decks[attackerIndex].Empty)
                        Console.WriteLine($"{decks[attackerIndex].Owner} won the game with {decks[attackerIndex].Count} in the deck");
                    else
                        Console.WriteLine($"{decks[defenderIndex].Owner} won the game with {decks[defenderIndex].Count} in the deck");

                    return;
                }

                Console.WriteLine("-------------------------------------------------------------\n");
                Swap(ref attackerIndex, ref defenderIndex);
            }

            Console.WriteLine("DRAW");
        }

        private void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        public void EventListener(object publisher, RequestEventArgs args)
        {
            Console.WriteLine(args.Test);
        }

        public void HandleRequest(Request request)
        {
            switch(request.Values[""])
            {

            }

        }
    }
}
