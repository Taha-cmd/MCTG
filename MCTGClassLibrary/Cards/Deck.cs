using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary
{
    public class Deck
    {
        private List<Card> cards;
        public bool Empty { get { return cards.Count == 0; } }
        public int Size { get; private set; } 
        public int Count { get { return cards.Count;  } }
        public string Owner { get; protected set; }


        public Deck(int size, string owner = "anonym")
        {
            Owner = owner;
            Size = size;
            cards = new List<Card>();
        }

        public void Add(Card newCard)
        {
            if (cards.Count == Size)
                throw new DataException("Deck is full");

            cards.Add(newCard);
        }

        public void Extend(Card newCard)
        {
            cards.Add(newCard);
        }

        public Card GetRandomCard()
        {
            if (Empty)
                throw new DataException("Deck is empty");

            var random = new Random();
            return cards[ random.Next(0, cards.Count) ];
        }

        public void Clear()
        {
            cards.Clear();
        }

        public void Remove(Card target)
        {
            cards.Remove(target);
        }

    }
}
