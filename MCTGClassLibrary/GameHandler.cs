using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public class GameHandler
    {
        public GameHandler() { }

        private Queue< Player > queue = new Queue< Player >();

        public void EnqueuePlayer(Player player)
        {
            queue.Enqueue(player);
        }

        private void StartBattle(Player player1, Player player2)
        {

        }

        public void EventListener(object publisher, RequestEventArgs args)
        {
            Console.WriteLine(args.Test);

        }


    }
}
