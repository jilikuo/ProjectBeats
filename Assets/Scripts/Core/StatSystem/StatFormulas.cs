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
                default:
                    return new List<Attribute>();
            }
        }


        public static float CalculateBaseStatValue(StatType Type, List<Attribute> list)
        {
            if (list == null)
                throw new ArgumentNullException("Cannot calculate base stat value for Stat Type " + Type.ToString() + ", Attribute list is null.");

            switch (Type)
            {
                // Attack Damage = 10 * Strength
                case StatType.AttackDamage:
                    float str = list.Find(Attribute => Attribute.Type == AttributeType.Strength).Value;
                    return 10 * str;

                // Health = 100 + 25 * Constitution
                case StatType.Health:
                    float con = list.Find(Attribute => Attribute.Type == AttributeType.Constitution).Value;
                    return 100 + 25 * con;

                // Armor = 5 * Resistance
                case StatType.Armor:
                    float res = list.Find(Attribute => Attribute.Type == AttributeType.Resistance).Value;
                    return 5 * res;

                // Movement Speed = 1 + 0.5 * Dextery
                case StatType.MovementSpeed:
                    float dex = list.Find(Attribute => Attribute.Type == AttributeType.Dextery).Value;
                    return 1 + (dex * 0.5f);

                // Acceleration = 0.01 * Agility    
                case StatType.Acceleration:
                    float agi = list.Find(Attribute => Attribute.Type == AttributeType.Agility).Value;
                    return 0.1f + 0.05f * agi;

                default:
                    throw new ArgumentNullException("Cannot calculate base stat value for Stat Type " + Type.ToString() +  ", Type not found");
            }
        }
    }
}