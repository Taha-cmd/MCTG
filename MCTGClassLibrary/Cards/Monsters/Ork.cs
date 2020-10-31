using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.Cards.Monsters
{
    public class Ork : MonsterCard
    {
        public Ork(ElementType elementType = ElementType.Normal) : base(elementType, MonsterType.Ork)
        {

        }

        public Ork(CardData data, ElementType elementType = ElementType.Normal) : base(data, elementType, MonsterType.Ork)
        {

        }

        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            double battleDamage = Damage;

            switch (enemy.MonsterType)
            {
                case MonsterType.Goblin:
                    break;

                case MonsterType.Dragon:
                    break;

                case MonsterType.Wizzard:
                    return false;

                case MonsterType.Ork:
                    break;

                case MonsterType.Knight:
                    break;

                case MonsterType.Kraken:
                    break;

                case MonsterType.FireElf:
                    break;
            }


            return battleDamage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            double battleDamage = Damage;

            switch (enemy.ElementType)
            {
                case ElementType.Fire:
                    break;

                case ElementType.Water:
                    break;

                case ElementType.Normal:
                    break;
            }

            battleDamage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, battleDamage);

            return battleDamage > enemy.Damage;
        }
    }
}
