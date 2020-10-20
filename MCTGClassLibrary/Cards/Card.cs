using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary
{
    public abstract class Card
    {
        public string Name { get; protected set; }
        public double Damage { get; set; }
        public ElementType ElementType { get; protected set; }
        public CardType CardType { get; protected set; }

        public Card(ElementType element, CardType cardType)
        {
            ElementType = element;
            CardType = cardType;
        }

        protected bool Effective(ElementType attacker, ElementType defender)
        {
            //  rules: 
            //  water -> fire
            //  fire -> normal
            //  normal -> water

            return
            (
                (attacker == ElementType.Water  && defender == ElementType.Fire)
                                                ||
                (attacker == ElementType.Fire   && defender == ElementType.Normal)
                                                ||
                (attacker == ElementType.Normal && defender == ElementType.Water)
            );

        }

        protected double CalcualteDamageBasedOnElementTypeEffectiveness(ElementType attacker, ElementType defender, double attackerDamage)
        {

            if (attacker != defender)
            {
                if (Effective(attacker, defender))
                    attackerDamage *= 2;
                else
                    attackerDamage /= 2;
            }

            return attackerDamage;
        }

        public bool Attack(Card enemy)
        {
            return enemy.CardType == CardType.Monster ? AttackMonster(enemy) : AttackSpell(enemy);
        }

        public abstract string Describe();
        protected abstract bool AttackMonster(Card enemy);
        protected abstract bool AttackSpell(Card enemy);

    }
}
