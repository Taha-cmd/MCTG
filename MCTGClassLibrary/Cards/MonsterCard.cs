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
        public override string Description()
        {
            return $"Name: {Name}\nType: {CardType.ToString()}\nKind: {MonsterType.ToString()}\nElement: {ElementType.ToString()}\nDamage: {Damage}\n";
        }
    }
}
