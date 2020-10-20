using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary
{
    public class SpellCard : Card
    {
        public SpellCard(ElementType element = ElementType.Normal) : base(element, CardType.Spell)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Card card = obj as Card;

            if (card == null) return false;
            if (card.CardType == CardType.Monster) return false;
            if (card.ElementType != ElementType) return false;

            return true;
        }

        public override string Describe()
        {
            return $"({ElementType.ToString()}, {CardType.ToString()})";
        }

        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            Damage = enemy.Damage = 1;

            if (enemy.MonsterType == MonsterType.Kraken)
                return false;

            if (enemy.MonsterType == MonsterType.Knight && ElementType == ElementType.Water)
                return true;

            Damage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, Damage);

            return Damage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            Damage = enemy.Damage = 1;

            Damage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, Damage);
                
            return Damage > enemy.Damage;
        }
    }
}
