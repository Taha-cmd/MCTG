using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MCTGClassLibrary;
using System.Data;
using MCTGClassLibrary.Enums;
using MCTGClassLibrary.Cards;

namespace MCTGUnitTests
{
    class DeckTestClass
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void TestFullDeck()
        {
            Deck deck = new Deck(2);

            deck.Add(CardsManager.RandomCard());
            deck.Add(CardsManager.RandomCard());

            Assert.Throws<DataException>(() => deck.Add(CardsManager.RandomCard()));
        }


        [Test]
        public void TestGetRandomCard()
        {
            Deck deck = new Deck(4);

            deck.Add(CardsManager.RandomMonster());
            deck.Add(CardsManager.RandomSpell());
            deck.Add(CardsManager.RandomMonster());
            deck.Add(CardsManager.RandomSpell());

            for (int i = 0; i < 10000; i++)
            {
                Assert.DoesNotThrow(() => deck.GetRandomCard());
            }
            // test that GetRandomCard doesn't go out of boundaries
        }

        [Test]
        public void TestEmptyDeck()
        {
            Deck deck = new Deck(5);

            Assert.Throws<DataException>(() => deck.GetRandomCard());

        }

        [Test]
        public void TestRemove()
        {
            Deck deck = new Deck(50);

            // test the equality operator

            for(int i = 0; i < 10000; i++)
            {
                Card card = CardsManager.RandomCard();

                deck.Add(card);
                deck.Remove(card);

                Assert.IsTrue(deck.Empty);
            }
        }
    }
}
