using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary.Cards.Monsters
{
    public class Dragon : MonsterCard
    {

        public Dragon(ElementType elementType = ElementType.Normal) : base(elementType, MonsterType.Dragon)
        {

        }

        public Dragon(CardData data, ElementType elementType = ElementType.Normal) : base(data, elementType, MonsterType.Dragon)
        {

        }

        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            double battleDamage = Damage;

            switch (enemy.MonsterType)
            {
                case MonsterType.Goblin:
                    return true;

                case MonsterType.Dragon:
                    break;

                case MonsterType.Wizzard:
                    break;

                case MonsterType.Ork:
                    break;

                case MonsterType.Knight:
                    break;

                case MonsterType.Kraken:
                    break;

                case MonsterType.FireElf:
                    return false;
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
