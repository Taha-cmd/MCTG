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
            CardType cardType = ExtractCardType(name);

            if(cardType == CardType.Spell)
                return new SpellCard(data, elementType);
            else
            {
                switch (ExtractMonsterType(name))
                {
                    case MonsterType.Goblin:    return new Goblin(data, elementType);
                    case MonsterType.Dragon:    return new Dragon(data, elementType);
                    case MonsterType.Wizzard:   return new Wizzard(data, elementType);
                    case MonsterType.Ork:       return new Ork(data, elementType);
                    case MonsterType.Knight:    return new Knight(data, elementType);
                    case MonsterType.Kraken:    return new Kraken(data, elementType);
                    case MonsterType.FireElf:   return new FireElf(data, elementType);
                }
            }

            return null;
        }

        public static ElementType ExtractElementType(string name)
        {
            name = name.ToLower();

            if (name.Contains("water")) return ElementType.Water;
            if (name.Contains("fire"))  return ElementType.Fire;

            return ElementType.Normal;
        }

        public static CardType ExtractCardType(string name)
        {
            name = name.ToLower();

            if (name.Contains("spell")) return CardType.Spell;

            return CardType.Monster;
        }

        public static MonsterType ExtractMonsterType(string name)
        {
            name = name.ToLower();

            if (name.Contains("dragon"))    return MonsterType.Dragon;
            if (name.Contains("fireelf"))   return MonsterType.FireElf;
            if (name.Contains("goblin"))    return MonsterType.Goblin;
            if (name.Contains("knight"))    return MonsterType.Knight;
            if (name.Contains("kraken"))    return MonsterType.Kraken;
            if (name.Contains("ork"))       return MonsterType.Ork;

            // only wizzard left
            return MonsterType.Wizzard;
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
