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

            {
                Card attacker = new SpellCard(ElementType.Water);
                Card defender = new SpellCard(ElementType.Fire);
                Assert.IsTrue(attacker.Attack(defender));
            }

            {
                Card attacker = new SpellCard(ElementType.Fire);
                Card defender = new SpellCard(ElementType.Normal);
                Assert.IsTrue(attacker.Attack(defender));
            }


            {
                Card attacker = new SpellCard(ElementType.Normal);
                Card defender = new SpellCard(ElementType.Water);
                Assert.IsTrue(attacker.Attack(defender));
            }


        }


        [Test]
        public void TestIneffectiveSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            // attacker must lose

            {
                Card attacker = new SpellCard(ElementType.Normal);
                Card defender = new SpellCard(ElementType.Fire);
                Assert.IsFalse(attacker.Attack(defender));
            }

            {
                Card attacker = new SpellCard(ElementType.Water);
                Card defender = new SpellCard(ElementType.Normal);
                Assert.IsFalse(attacker.Attack(defender));
            }


            {
                Card attacker = new SpellCard(ElementType.Fire);
                Card defender = new SpellCard(ElementType.Water);
                Assert.IsFalse(attacker.Attack(defender));
            }
        }

        [Test]
        public void TestNoEffectivenessSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            // attacker must lose

            {
                Card attacker = new SpellCard(ElementType.Normal);
                Card defender = new SpellCard(ElementType.Normal);
                Assert.IsFalse(attacker.Attack(defender));
            }

            {
                Card attacker = new SpellCard(ElementType.Water);
                Card defender = new SpellCard(ElementType.Water);
                Assert.IsFalse(attacker.Attack(defender));
            }


            {
                Card attacker = new SpellCard(ElementType.Fire);
                Card defender = new SpellCard(ElementType.Fire);
                Assert.IsFalse(attacker.Attack(defender));
            }
        }


        [Test]
        public void TestSpellCardAgainstKraken()
        {
            MonsterCard monster = new MonsterCard(ElementType.Normal, MonsterType.Kraken);

            {
                Card spell = new SpellCard(ElementType.Fire);
                Assert.IsFalse(spell.Attack(monster));
            }

            {
                Card spell = new SpellCard(ElementType.Water);
                Assert.IsFalse(spell.Attack(monster));
            }

            {
                Card spell = new SpellCard(ElementType.Normal);
                Assert.IsFalse(spell.Attack(monster));
            }

        }

        [Test]
        public void TestWaterSpellCardAgainstKnights()
        {
            SpellCard spell = new SpellCard(ElementType.Water);
            MonsterCard monster = new MonsterCard(ElementType.Normal, MonsterType.Knight);

            Assert.IsTrue(spell.Attack(monster));

        }


    }
}
