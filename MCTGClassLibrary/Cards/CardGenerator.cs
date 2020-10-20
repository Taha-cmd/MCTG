using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.Cards.Monsters;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.Cards
{
    public class CardGenerator
    {
        private static Random random = new Random();
        public static Card RandomCard()
        {
            CardType cardType = (CardType)random.Next(0, 2);

            return cardType == CardType.Monster ? RandomMonster() : RandomSpell();
        }

        public static Card RandomMonster()
        {
            MonsterType monsterType = (MonsterType)random.Next(0, 7);
            ElementType elementType = (ElementType)random.Next(0, 3);

            switch (monsterType)
            {
                case MonsterType.Dragon:    return new Dragon(elementType);
                case MonsterType.Goblin:    return new Goblin(elementType);
                case MonsterType.Wizzard:   return new Wizzard(elementType);
                case MonsterType.Ork:       return new Ork(elementType);
                case MonsterType.Knight:    return new Knight(elementType);
                case MonsterType.Kraken:    return new Kraken(elementType);
                case MonsterType.FireElve:  return new FireElve(elementType);
            }

            return null;
        }

        public static Card RandomSpell()
        {
            ElementType elementType = (ElementType)random.Next(0, 3);
            return new SpellCard(elementType);
        }
    }
}
