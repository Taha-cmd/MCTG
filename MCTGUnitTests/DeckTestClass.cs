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

            deck.Add(CardGenerator.RandomCard());
            deck.Add(CardGenerator.RandomCard());

            Assert.Throws<DataException>(() => deck.Add(CardGenerator.RandomCard()));
        }


        [Test]
        public void TestGetRandomCard()
        {
            Deck deck = new Deck(4);

            deck.Add(CardGenerator.RandomMonster());
            deck.Add(CardGenerator.RandomSpell());
            deck.Add(CardGenerator.RandomMonster());
            deck.Add(CardGenerator.RandomSpell());

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

            deck.Add(new SpellCard(ElementType.Normal));
            Assert.IsFalse(deck.Empty);

            deck.Remove(new SpellCard(ElementType.Water));
            Assert.IsFalse(deck.Empty);

            deck.Remove(new SpellCard(ElementType.Normal));
            Assert.IsTrue(deck.Empty);
        }
    }
}
