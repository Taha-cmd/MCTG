using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary;
using NUnit.Framework;

namespace MCTGUnitTests
{
    class SpellCardTestClass
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void TestEffectiveSpellCardFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            // attacker must win

            Card attacker = new SpellCard();
            Card defender = new SpellCard();

            attacker.ElementType = ElementType.Water;
            defender.ElementType = ElementType.Fire;
            Assert.IsTrue(attacker.Attack(defender));

            attacker.ElementType = ElementType.Fire;
            defender.ElementType = ElementType.Normal;
            Assert.IsTrue(attacker.Attack(defender));

            attacker.ElementType = ElementType.Normal;
            defender.ElementType = ElementType.Water;
            Assert.IsTrue(attacker.Attack(defender));

        }


        [Test]
        public void TestIneffectiveSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            // attacker must lose

            Card attacker = new SpellCard();
            Card defender = new SpellCard();

            attacker.ElementType = ElementType.Normal;
            defender.ElementType = ElementType.Fire;
            Assert.IsFalse(attacker.Attack(defender));

            attacker.ElementType = ElementType.Water;
            defender.ElementType = ElementType.Normal;
            Assert.IsFalse(attacker.Attack(defender));

            attacker.ElementType = ElementType.Fire;
            defender.ElementType = ElementType.Water;
            Assert.IsFalse(attacker.Attack(defender));

        }

        [Test]
        public void TestNoEffectivenessSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            // attacker must lose

            Card attacker = new SpellCard();
            Card defender = new SpellCard();

            attacker.ElementType = ElementType.Normal;
            defender.ElementType = ElementType.Normal;
            Assert.IsFalse(attacker.Attack(defender));

            attacker.ElementType = ElementType.Water;
            defender.ElementType = ElementType.Water;
            Assert.IsFalse(attacker.Attack(defender));

            attacker.ElementType = ElementType.Fire;
            defender.ElementType = ElementType.Fire;
            Assert.IsFalse(attacker.Attack(defender));

        }


        [Test]
        public void TestSpellCardAgainstKraken()
        {
            SpellCard spell = new SpellCard();
            MonsterCard monster = new MonsterCard();
            monster.MonsterType = MonsterType.Kraken;

            spell.ElementType = ElementType.Fire;
            Assert.IsFalse(spell.Attack(monster));

            spell.ElementType = ElementType.Water;
            Assert.IsFalse(spell.Attack(monster));

            spell.ElementType = ElementType.Normal;
            Assert.IsFalse(spell.Attack(monster));
        }

        [Test]
        public void TestWaterSpellCardAgainstKnights()
        {
            SpellCard spell = new SpellCard();
            MonsterCard monster = new MonsterCard();
            monster.MonsterType = MonsterType.Knight;

            spell.ElementType = ElementType.Water;
            Assert.IsTrue(spell.Attack(monster));

        }


    }
}
