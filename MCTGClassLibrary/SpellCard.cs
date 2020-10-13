using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public class SpellCard : Card
    {
        public SpellCard()
        {
            CardType = CardType.Spell;
        }
        public override bool Attack(Card enemy)
        {
            return enemy.CardType == CardType.Monster ? AttackMonster(enemy) : AttackSpell(enemy);
        }

        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            Damage = enemy.Damage = 1;

            if (enemy.MonsterType == MonsterType.Kraken)
                return false;

            if (enemy.MonsterType == MonsterType.Knight)
                if (ElementType == ElementType.Water)
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
