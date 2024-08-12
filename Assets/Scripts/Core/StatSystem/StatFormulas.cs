using System;
using System.Collections.Generic;

namespace Jili.StatSystem
{
    public static class StatFormulas
    {
        public static List<Attribute> FilterRelevantAttributes(StatType Type, List<Attribute> list)
        {
            switch (Type)
            {
                case StatType.AttackDamage:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Strength);
                case StatType.Health:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Constitution);
                case StatType.Armor:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Resistance);
                case StatType.Acceleration:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Agility);
                case StatType.MovementSpeed:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Dextery);
                case StatType.AttacksPerSecond:
                    return list.FindAll(Attribute => (Attribute.Type == AttributeType.Dextery) ||
                                                     (Attribute.Type == AttributeType.Finesse));
                default:
                    return new List<Attribute>();
            }
        }


        public static float CalculateBaseStatValue(StatType Type, List<Attribute> list)
        {
            if (list == null)
                throw new ArgumentNullException("Cannot calculate base stat value for Stat Type " + Type.ToString() + ", Attribute list is null.");

            float str = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Strength) == null)
            {

            }
            else
            {
                str = list.Find(Attribute => Attribute.Type == AttributeType.Strength).Value;
            }

            float con = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Constitution) == null)
            {

            }
            else
            {
                con = list.Find(Attribute => Attribute.Type == AttributeType.Constitution).Value;
            }

            float res = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Resistance) == null)
            {

            }
            else
            {
                res = list.Find(Attribute => Attribute.Type == AttributeType.Resistance).Value;
            }

            float dex = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Dextery) == null)
            {

            }
            else
            {
                dex = list.Find(Attribute => Attribute.Type == AttributeType.Dextery).Value;
            }

            float agi = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Agility) == null)
            {

            }
            else
            {
                agi = list.Find(Attribute => Attribute.Type == AttributeType.Agility).Value;
            }

            float fin = 0;
            if (list.Find(Attribute => Attribute.Type == AttributeType.Finesse) == null)
            {

            }
            else
            {
                fin = list.Find(Attribute => Attribute.Type == AttributeType.Finesse).Value;
            }

            switch (Type)
            {
                // Attack Damage = 10 * Strength
                case StatType.AttackDamage:

                    return 10 * str;

                // Health = 100 + 25 * Constitution
                case StatType.Health:
                    return 100 + 25 * con;

                // Armor = 5 * Resistance
                case StatType.Armor:
                    return 5 * res;

                // Movement Speed = 1 + 0.5 * Dextery
                case StatType.MovementSpeed:
                    return 1 + (dex * 0.5f);

                // Acceleration = 0.01 * Agility    
                case StatType.Acceleration:
                    return 0.1f + 0.05f * agi;

                // Attacks Per Second = 0.333 + 0.333 * Dextery/100 + 0.333 * Finesse/10
                case StatType.AttacksPerSecond:
                    return 0.333f + 0.333f * (dex / 100) + 0.333f * (fin / 10);

                default:
                    throw new ArgumentNullException("Cannot calculate base stat value for Stat Type " + Type.ToString() +  ", Type not found");
            }
        }
    }
}