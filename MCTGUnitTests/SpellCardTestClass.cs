using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary;
using NUnit.Framework;
using MCTGClassLibrary.Enums;
using MCTGClassLibrary.Cards.Monsters;
using MCTGClassLibrary.Cards;

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

            for(int i = 0; i < 10000; i++)
            {
                {
                    Card attacker = new SpellCard(ElementType.Water);
                    Card defender = new SpellCard(ElementType.Fire);

                    double attackerDamage = attacker.Damage * 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }

                {
                    Card attacker = new SpellCard(ElementType.Fire);
                    Card defender = new SpellCard(ElementType.Normal);

                    double attackerDamage = attacker.Damage * 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }


                {
                    Card attacker = new SpellCard(ElementType.Normal);
                    Card defender = new SpellCard(ElementType.Water);

                    double attackerDamage = attacker.Damage * 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }

            }
        }


        [Test]
        public void TestIneffectiveSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            for(int i = 0; i < 10000; i++)
            {
                {
                    Card attacker = new SpellCard(ElementType.Normal);
                    Card defender = new SpellCard(ElementType.Fire);

                    double attackerDamage = attacker.Damage / 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }

                {
                    Card attacker = new SpellCard(ElementType.Water);
                    Card defender = new SpellCard(ElementType.Normal);

                    double attackerDamage = attacker.Damage / 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }


                {
                    Card attacker = new SpellCard(ElementType.Fire);
                    Card defender = new SpellCard(ElementType.Water);

                    double attackerDamage = attacker.Damage / 2;
                    Assert.AreEqual(attacker.Attack(defender), attackerDamage > defender.Damage);
                }

            }
        }

        [Test]
        public void TestNoEffectivenessSpellCardsFight()
        {
            //  rules:
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            for(int i = 0; i < 10000; i++)
            {
                {
                    Card attacker = new SpellCard(ElementType.Normal);
                    Card defender = new SpellCard(ElementType.Normal);
                    Assert.AreEqual(attacker.Attack(defender), attacker.Damage > defender.Damage);
                }

                {
                    Card attacker = new SpellCard(ElementType.Water);
                    Card defender = new SpellCard(ElementType.Water);
                    Assert.AreEqual(attacker.Attack(defender), attacker.Damage > defender.Damage);
                }


                {
                    Card attacker = new SpellCard(ElementType.Fire);
                    Card defender = new SpellCard(ElementType.Fire);
                    Assert.AreEqual(attacker.Attack(defender), attacker.Damage > defender.Damage);
                }

            }

        }


        [Test]
        public void TestSpellCardAgainstKraken()
        {
            MonsterCard monster = new Kraken();

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
            MonsterCard monster = new Knight();

            Assert.IsTrue(spell.Attack(monster));

        }


    }
}
