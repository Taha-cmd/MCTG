using System;
using System.Collections.Generic;
using System.Text;
using MCTGClassLibrary.Cards.Monsters;
using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary.Cards
{
    public class CardsManager
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
            CardData data = RandomCardData();

            switch (monsterType)
            {
                case MonsterType.Dragon:    return new Dragon(data, elementType);
                case MonsterType.Goblin:    return new Goblin(data, elementType);
                case MonsterType.Wizzard:   return new Wizzard(data, elementType);
                case MonsterType.Ork:       return new Ork(data, elementType);
                case MonsterType.Knight:    return new Knight(data, elementType);
                case MonsterType.Kraken:    return new Kraken(data, elementType);
                case MonsterType.FireElf:   return new FireElf(data, elementType);
            }

            return null;
        }

        public static Card RandomSpell()
        {
            ElementType elementType = (ElementType)random.Next(0, 3);
            CardData data = RandomCardData();

            return new SpellCard(data, elementType);
        }

        public static Card Create(CardData data)
        {
            string name = data.Name.ToLower();

            ElementType elementType = ExtractElementType(name);

            if (name.Contains("spell"))     return new SpellCard(data, elementType);

            if (name.Contains("dragon"))    return new Dragon(data, elementType);
            if (name.Contains("fireelf"))   return new FireElf(data, elementType);
            if (name.Contains("goblin"))    return new Goblin(data, elementType);
            if (name.Contains("knight"))    return new Knight(data, elementType);
            if (name.Contains("kraken"))    return new Kraken(data, elementType);
            if (name.Contains("ork"))       return new Ork(data, elementType);
            if (name.Contains("wizzard"))   return new Wizzard(data, elementType);

            return null;

        }

        private static ElementType ExtractElementType(string name)
        {
            if (name.Contains("water")) return ElementType.Water;
            if (name.Contains("fire")) return ElementType.Fire;

            return ElementType.Normal;
        }

        public static CardData RandomCardData()
        {
            string id = Guid.NewGuid().ToString();
            string name = "Random";
            double damage = random.NextDouble() * 100;
            double weakness = random.NextDouble() * 100;

            return new CardData(id, name, damage, weakness);
        }
    }
}
