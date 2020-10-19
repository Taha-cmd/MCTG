using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace MCTGClassLibrary
{
    public class RandomCard
    {
        public static Card Get()
        {
            Random random = new Random();

            CardType cardType = (CardType)random.Next(0, 2);
            ElementType elementType = (ElementType)random.Next(0, 3);

            if(cardType == CardType.Monster)
            {
                MonsterType monsterType = (MonsterType)random.Next(0, 7);
                return new MonsterCard(elementType, monsterType);
            }

            return new SpellCard(elementType);
        }
    }
}
