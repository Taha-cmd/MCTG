using System;
using System.Collections.Generic;
using System.Text;

namespace MCTGClassLibrary
{
    public enum MonsterType
    {
        Goblin,
        Dragon,
        Wizzard,
        Ork,
        Knight,
        Kraken,
        FireElve
    }
    public class MonsterCard : Card
    {
        public MonsterType MonsterType { get; protected set; }

        public MonsterCard(ElementType elementType, MonsterType monsterType) : base(elementType, CardType.Monster)
        {
            MonsterType = monsterType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Card card = obj as Card;

            if (card == null) return false;
            if (card.CardType == CardType.Spell) return false;
            if (card.ElementType != ElementType) return false;

            MonsterCard monster = obj as MonsterCard;

            if (monster == null) return false;
            if (monster.MonsterType != MonsterType) return false;

            return true;
        }

        public override string Describe()
        {
            return $"({ElementType.ToString()}, {CardType.ToString()}, {MonsterType.ToString()})";
        }

        public override bool Attack(Card enemy)
        {
            return enemy.CardType == CardType.Monster ? AttackMonster(enemy) : AttackSpell(enemy);
        }


        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            Damage = enemy.Damage = 1;

            switch(MonsterType)
            {
                case MonsterType.Goblin:
                    if (enemy.MonsterType == MonsterType.Dragon) return false;
                    break;                  

                case MonsterType.Dragon:
                    if (enemy.MonsterType == MonsterType.Goblin) return true;
                    if (enemy.MonsterType == MonsterType.FireElve) return false;
                    break;

                case MonsterType.Wizzard:
                    if (enemy.MonsterType == MonsterType.Ork) return true;
                    break;

                case MonsterType.Ork:
                    if (enemy.MonsterType == MonsterType.Wizzard) return false;
                    break;

                case MonsterType.Knight:
                    break;

                case MonsterType.Kraken:
                    break;

                case MonsterType.FireElve:
                    if (enemy.MonsterType == MonsterType.Dragon) return true;
                    break;
            }


            return Damage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            Damage = enemy.Damage = 1;

            switch (MonsterType)
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
                    if (enemy.ElementType == ElementType.Water) return false;
                    break;

                case MonsterType.Kraken:
                        return true;

                case MonsterType.FireElve:

                    break;
            }

            Damage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, Damage);

            return Damage > enemy.Damage;
        }
    }
}
