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
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Strength);
                case StatType.MovementSpeed:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Agility);
                case StatType.Acceleration:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Agility);
                default:
                    return new List<Attribute>();
            }
        }


        public static float CalculateBaseStatValue(StatType Type, List<Attribute> list)
        {
            float result;
            switch (Type)
            {
                case StatType.AttackDamage:
                    float str = list.Find(Attribute => Attribute.Type == AttributeType.Strength).Value;
                    result = 10 * str;
                    break;
                case StatType.Health:
                    float con = list.Find(Attribute => Attribute.Type == AttributeType.Constitution).Value;
                    result = 100 + 25 * con;
                    break;
                case StatType.Armor:
                    result = 10;
                    break;
                case StatType.MovementSpeed:
                    result = 10;
                    break;
                case StatType.Acceleration:
                    result = 10;
                    break;
                default:
                    result = 0;
                    break;
            }
            return result;
        }
    }
}