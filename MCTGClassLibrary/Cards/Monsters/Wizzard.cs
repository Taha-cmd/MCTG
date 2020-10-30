using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.Cards.Monsters
{
    public class Wizzard : MonsterCard
    {
        public Wizzard(ElementType elementType = ElementType.Normal) : base(elementType, MonsterType.Wizzard)
        {

        }

        public Wizzard(CardData data, ElementType elementType = ElementType.Normal) : base(data, elementType, MonsterType.Wizzard)
        {

        }
        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            Damage = enemy.Damage = 1;

            switch (enemy.MonsterType)
            {
                case MonsterType.Goblin:
                    break;

                case MonsterType.Dragon:
                    break;

                case MonsterType.Wizzard:
                    break;

                case MonsterType.Ork:
                    return true;

                case MonsterType.Knight:
                    break;

                case MonsterType.Kraken:
                    break;

                case MonsterType.FireElf:
                    break;
            }


            return Damage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            Damage = enemy.Damage = 1;

            switch (enemy.ElementType)
            {
                case ElementType.Fire:
                    break;

                case ElementType.Water:
                    break;

                case ElementType.Normal:
                    break;
            }

            Damage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, Damage);

            return Damage > enemy.Damage;
        }
    }
}
