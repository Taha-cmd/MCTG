using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using MCTGClassLibrary;
using MCTGClassLibrary.Enums;
using MCTGClassLibrary.Cards.Monsters;

namespace MCTGUnitTests
{
    class MonsterCardTestClass
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void TestGoblinVsDragon()
        {
            //Goblins are too afraid of Dragons to attack. => dragons always win
            Card goblin = new Goblin();
            Card dragon = new Dragon();

            Assert.IsTrue(dragon.Attack(goblin));
            Assert.IsFalse(goblin.Attack(dragon));
        }

        [Test]
        public void TestWizzardVsOrk()
        {
            // Wizzard can control Orks so they are not able to damage them. => wizzard always win
            Card wizzard = new Wizzard();
            Card ork = new Ork();

            Assert.IsTrue(wizzard.Attack(ork));
            Assert.IsFalse(ork.Attack(wizzard));

        }

        [Test]
        public void TestKnightVsWaterSpellCard()
        {
            // The armor of Knights is so heavy that WaterSpells make them drown them
            //instantly. => water spell card always wins

            Card dragon = new Knight();
            Card water = new SpellCard(ElementType.Water);

            Assert.IsTrue(water.Attack(dragon));
            Assert.IsFalse(dragon.Attack(water));

        }

        [Test]
        public void TestKrakenVsSpells()
        {
            // The Kraken is immune against spells. => kraken wins against all spells
            Card kraken = new Kraken();

            Card water = new SpellCard(ElementType.Water);
            Card normal = new SpellCard(ElementType.Normal);
            Card fire = new SpellCard(ElementType.Fire);

            Assert.IsTrue(kraken.Attack(water));
            Assert.IsTrue(kraken.Attack(normal));
            Assert.IsTrue(kraken.Attack(fire));

            Assert.IsFalse(water.Attack(kraken));
            Assert.IsFalse(normal.Attack(kraken));
            Assert.IsFalse(fire.Attack(kraken));

        }

        [Test]
        public void TestFireElvesVsDragons()
        {
            //The FireElves know Dragons since they were little and can evade their attacks. => FireElves always win vs Dragons

            Card dragon = new Dragon();
            Card elve = new FireElf();

            Assert.IsTrue(elve.Attack(dragon));
            Assert.IsFalse(dragon.Attack(elve));

        }
    }
}
