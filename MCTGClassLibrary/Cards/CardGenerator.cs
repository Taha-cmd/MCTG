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

        public static Card Create(string name)
        {
            name = name.ToLower();

            ElementType elementType = ExtractElementType(name);

            if (name.Contains("spell"))     return new SpellCard(elementType);

            if (name.Contains("dragon"))    return new Dragon(elementType);
            if (name.Contains("fireelve"))  return new FireElve(elementType);
            if (name.Contains("goblin"))    return new Goblin(elementType);
            if (name.Contains("knight"))    return new Knight(elementType);
            if (name.Contains("kraken"))    return new Kraken(elementType);
            if (name.Contains("ork"))       return new Ork(elementType);
            if (name.Contains("wizzard"))   return new Wizzard(elementType);

            return null;

        }

        private static ElementType ExtractElementType(string name)
        {
            if (name.Contains("water")) return ElementType.Water;
            if (name.Contains("fire")) return ElementType.Fire;

            return ElementType.Normal;
        }
    }
}
