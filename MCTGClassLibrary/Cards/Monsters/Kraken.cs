using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.Cards.Monsters
{
    public class Kraken : MonsterCard
    {
        public Kraken(ElementType elementType = ElementType.Normal) : base(elementType, MonsterType.Kraken)
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
                    break;

                case MonsterType.Knight:
                    break;

                case MonsterType.Kraken:
                    break;

                case MonsterType.FireElve:
                    break;
            }


            return Damage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            return true;
        }
    }
}
