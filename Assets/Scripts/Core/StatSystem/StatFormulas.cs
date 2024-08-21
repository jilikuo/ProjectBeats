using System;
using System.Collections.Generic;

namespace Jili.StatSystem
{
    /*    Attributes and Stats Relationship      // atributos e status desfrutam de um relacionamento N:N, onde um atributo pode influenciar em vários stats,
     * | ==================================== |  // e um status pode ser influenciado por vários atributos.
     * |     ATTRIBUTES  |      STATS         |  // stats podem possuir valores base (por exemplo 1 em movespeed), 
     * | ======== PHYSICAL STATS ============ |  // e o valor dos atributos somente devem "melhorar" o status.
     * | Strenght:        Attack Damage,      |
     * |                         ,            |
     * | Constitution:    Health,             |  // Atributos são valores base que podem ser modificados por itens, habilidades, ou efeitos temporários.
     * |                         ,            |
     * | Resistance:      Armor,              |
     * |                  ,                   |
     * | Vigor:           Health Regen,       |  // Stats também podem ser afetados por modificadores, assim como atributos, mas devem sempre receber
     * |                                      |  // o valor da fórmula de cálculo do status para ser usado como base.
     * | ======== MOBILITY STATS ============ |  
     * | Dextery:         Movement Speed,     |
     * |                  Attacks Per Second, |
     * | Agility:         Acceleration,       |
     * | Finesse:         Attacks Per Second, |
     * | Precision:       Attack Range        |
     * | ======== MAGICAL STATS ============= |
     * | Intelligence:    Magic Damage,       |
     * | Willpower:       Mana,               |
     * | Wisdom:          Magic Resistance,   |
     * | Spirit:          Mana Regen,         |
     * | ======== SOCIAL STATS ============== |
     * | Charisma:        ,                   |
     * | Influence:       ,                   |
     * | Leadership:      ,                   |
     * | Presence:        ,                   |
     * | ======== SPECIAL STATS ============= |
     * | Luck:            Critical Chance,    |
     * | Level:           Experience,         |
     * | Gold:            Currency,           |
     * | ==================================== |
     * | ======== IDEPENDENT STATS ========== |    // Stats independentes não possuem atributos relacionados, e são calculados de forma direta.
     * | Projectile Number,                   |
     * | Projectile Speed,                    |
     * | ==================================== |
    */
    public static class StatFormulas
    {
        public static List<Attribute> FilterRelevantAttributes(StatType Type, List<Attribute> list)
        {
            if ((int)Type >= (int)StatType.IndependentBase)
                throw new ArgumentOutOfRangeException("NÃO É POSSÍVEL FILTRAR ATRIBUTOS RELEVANTES DE UM STAT INDEPENDENTE");


            //TODO: SE NÃO ENCONTRAR O STATUS NA LISTA, PRECISA RETORNAR UM ERRO.
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
                case StatType.AttackRange:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Precision);
                case StatType.Mana:
                    return list.FindAll(Attribute => Attribute.Type == AttributeType.Willpower);

                default:
                    throw new ArgumentOutOfRangeException("NÃO HÁ FILTROS PROGRAMADOS PARA O STAT TIPO " + Type);
            }
        }


        public static float CalculateBaseStatValue(StatType Type, List<Attribute> list)
        {
            if ((int)Type >= (int)StatType.IndependentBase)
                throw new ArgumentOutOfRangeException("ERROR: YOU CANNOT CALCULATE BASE VALUES FOR AN INDEPENDENT STAT");
            if (list == null)
                throw new ArgumentNullException("Cannot calculate base stat value for Stat Type " + Type.ToString() + ", Attribute list is null.");
                    

            float str = 0; // Strenght
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Strength) == null))
                str = list.Find(Attribute => Attribute.Type == AttributeType.Strength).Value;

            float con = 0; // Constitution
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Constitution) == null))
                con = list.Find(Attribute => Attribute.Type == AttributeType.Constitution).Value;

            float res = 0; // Resistance
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Resistance) == null))
                res = list.Find(Attribute => Attribute.Type == AttributeType.Resistance).Value;

            float vig = 0; // Vigor
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Vigor) == null))
                vig = list.Find(Attribute => Attribute.Type == AttributeType.Vigor).Value;

            float dex = 0; // Dextery
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Dextery) == null))
                dex = list.Find(Attribute => Attribute.Type == AttributeType.Dextery).Value;

            float agi = 0; // Agility
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Agility) == null))
                agi = list.Find(Attribute => Attribute.Type == AttributeType.Agility).Value;

            float fin = 0; // Finesse
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Finesse) == null))
                fin = list.Find(Attribute => Attribute.Type == AttributeType.Finesse).Value;

            float prc = 0; // Precision
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Precision) == null))
                prc = list.Find(Attribute => Attribute.Type == AttributeType.Precision).Value;

            float intell = 0; // Intelligence
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Intelligence) == null))
                intell = list.Find(Attribute => Attribute.Type == AttributeType.Intelligence).Value;

            float will = 0; // Willpower
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Willpower) == null))
                will = list.Find(Attribute => Attribute.Type == AttributeType.Willpower).Value;

            float wis = 0; // Wisdom
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Wisdom) == null))
                wis = list.Find(Attribute => Attribute.Type == AttributeType.Wisdom).Value;

            float spr = 0; // Spirit
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Spirit) == null))
                spr = list.Find(Attribute => Attribute.Type == AttributeType.Spirit).Value;

            float chr = 0; // Charisma
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Charisma) == null))
                chr = list.Find(Attribute => Attribute.Type == AttributeType.Charisma).Value; 
            
            float lead = 0; // Leadership
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Leadership) == null))
                lead = list.Find(Attribute => Attribute.Type == AttributeType.Leadership).Value;

            float prs = 0; // Presence
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Presence) == null))
                prs = list.Find(Attribute => Attribute.Type == AttributeType.Presence).Value;

            float lck = 0; // Luck
            if (!(list.Find(Attribute => Attribute.Type == AttributeType.Luck) == null))
                lck = list.Find(Attribute => Attribute.Type == AttributeType.Luck).Value;

            switch (Type)
            {
                // Attack Damage = 10 * Strength
                case StatType.AttackDamage:
                    return 10 * str;

                // Health = 100 + 25 * Constitution
                case StatType.Health:
                    return 100 + 25 * con;

                case StatType.Mana:
                    return 50 + 5 * will;

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
                    return 0.333f + (0.34f * (dex / 100)) + (0.34f * (fin / 10));

                // Attack Range = 1 + 0.667 *  
                case StatType.AttackRange:
                    return 3 + (0.15f * prc);

                default:
                    throw new ArgumentOutOfRangeException("Cannot calculate base stat value for Stat Type " + Type.ToString() +  ", Type FORMULA not found");
            }
        }
    }
}