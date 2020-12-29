using MCTGClassLibrary.DataObjects;
using MCTGClassLibrary.Enums;

namespace MCTGClassLibrary
{
    public class SpellCard : Card
    {
        public SpellCard(ElementType element = ElementType.Normal) : base(element, CardType.Spell)
        {
        }

        public SpellCard(CardData data, ElementType element = ElementType.Normal)
            : base(data, element, CardType.Spell)
        {

        }

        public override string Description()
        {
            return $"Name: {Name}\nType: {CardType.ToString()}\nElement: {ElementType.ToString()}\nDamage: {Damage}\n";
        }

        protected override bool AttackMonster(Card monster)
        {
            MonsterCard enemy = (MonsterCard)monster;
            double battleDamage = Damage;

            if (enemy.MonsterType == MonsterType.Kraken)
                return false;

            if (enemy.MonsterType == MonsterType.Knight && ElementType == ElementType.Water)
                return true;

            battleDamage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, battleDamage);

            return battleDamage > enemy.Damage;
        }

        protected override bool AttackSpell(Card spell)
        {
            SpellCard enemy = (SpellCard)spell;
            double battleDamage = Damage;

            battleDamage = CalcualteDamageBasedOnElementTypeEffectiveness(ElementType, enemy.ElementType, battleDamage);
                
            return battleDamage > enemy.Damage;
        }
    }
}
