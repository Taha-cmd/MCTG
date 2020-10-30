using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary
{

    public abstract class MonsterCard : Card
    {
        public MonsterType MonsterType { get; protected set; }

        public MonsterCard(ElementType elementType, MonsterType monsterType) : base(elementType, CardType.Monster)
        {
            MonsterType = monsterType;
        }
        public MonsterCard(CardData data, ElementType elementType, MonsterType monsterType) : base(data, elementType, CardType.Monster)
        {
            MonsterType = monsterType;
        }

        /*public override bool Equals(object obj)
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
        } */

        public override string Describe()
        {
            return $"({ElementType.ToString()}, {CardType.ToString()}, {MonsterType.ToString()})";
        }
    }
}
