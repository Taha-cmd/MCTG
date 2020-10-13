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
        public MonsterType MonsterType { get; set; }

        public MonsterCard()
        {
            CardType = CardType.Monster;
        }

        public override bool Attack(Card enemy)
        {
            return enemy.CardType == CardType.Monster ? AttackMonster(enemy) : AttackSpell(enemy);
        }


        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            Damage = enemy.Damage = 1;


            //switch()


            return Damage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            Damage = enemy.Damage = 1;



            return Damage > enemy.Damage;
        }
    }
}
