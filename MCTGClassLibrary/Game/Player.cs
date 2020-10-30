using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MCTGClassLibrary
{
    public class Player
    {
        public Deck Deck { get; }
        public bool HasDeck { get { return !Deck.Empty; } }

        public string Name { get; protected set; }

        public Player(string name = "anonym")
        {
            Name = name;
            Deck = new Deck(4, name);
        }

        public void MakeDeck(Card c1, Card c2, Card c3, Card c4)
        {
            if (!Deck.Empty)
                Deck.Clear();

            Deck.Add(c1);
            Deck.Add(c2);
            Deck.Add(c3);
            Deck.Add(c4);
        }
    }
}
