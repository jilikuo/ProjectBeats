using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ConsumableType;

public interface IStat
{
    float CalculateStat(); // Método para calcular o valor do stat
}

public interface ISubStat
{
    IStat MainStat { get; } // Propriedade que referencia o stat principal
    float CurrentValue { get; set; } // O valor atual do substat, que pode mudar durante o jogo
}

public abstract class EntityStat : IStat
{
    protected List<EntityAttribute> _attributes;
    public float Value { get; protected set; } // Stores the last calculated value


    protected EntityStat(List<EntityAttribute> attributes)
    {
        _attributes = attributes;
        Value = CalculateStat(); // Initial calculation
    }

    public abstract float CalculateStat();

    public void Recalculate(float newValue)
    {
        Value = CalculateStat(); // Recalculate and store the new value
    }
}

public class PhysicalDamage : EntityStat
{
    public PhysicalDamage(Strength str, Resistance res) : base(new List<EntityAttribute> { str, res })
    {
        str.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        // Exemplo: força 95% + resistência 5%
        float strength = _attributes.FirstOrDefault(attr => attr.Name == "Strength")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        float result = 10 * ((strength * 0.95f) + (resistance * 0.05f));

        return result;
    }
}

public class PhysicalDefense : EntityStat
{
    public PhysicalDefense(Resistance res) : base(new List<EntityAttribute> { res })
    {
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        return resistance;
    }
}

public class PhysicalResistance : EntityStat
{
    public PhysicalResistance(Constitution con, Vigor vig) : base(new List<EntityAttribute> { con, vig })
    {
        con.OnValueChanged += Recalculate;
        vig.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        return 5 * constitution + (vigor + constitution) / 2;
    }
}

public class MaxHealth : EntityStat
{
    public MaxHealth(Constitution con) : base(new List<EntityAttribute> { con })
    {
        con.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        return 100 + constitution * 25;
    }
}

public class Health : ISubStat
{
    public IStat MainStat { get; private set; } // Referência ao `MaxHealth`
    public float CurrentValue { get; set; } // Valor atual da saúde

    public Health(IStat mainStat)
    {
        MainStat = mainStat;
        CurrentValue = MainStat.CalculateStat(); // Inicializa `Health` como `MaxHealth`
    }
}

public class HealthRegen : EntityStat
{
    public HealthRegen(Constitution con, Resistance res) : base(new List<EntityAttribute> { con, res })
    {
        con.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        return constitution * 0.5f + resistance * 0.5f;
    }
}


public class HealthRegenCD : EntityStat
{
    public HealthRegenCD(Constitution con, Resistance res, Vigor vig) : base(new List<EntityAttribute> { con, res, vig })
    {
        con.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
        vig.OnValueChanged += Recalculate;
    }
    public override float CalculateStat()
    {
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        return 20 - (constitution * 4 + resistance * 3 + vigor * 2 / 90);
    }
}

public class MaxStamina : EntityStat
{
    public MaxStamina(Vigor vig) : base(new List<EntityAttribute> { vig })
    {
        vig.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        return 100 + vigor * 10;
    }
}

public class Stamina : ISubStat
{
    public IStat MainStat { get; private set; }
    public float CurrentValue { get; set; }

    public Stamina(IStat mainStat)
    {
        MainStat = mainStat;
        CurrentValue = MainStat.CalculateStat();
    }
}

public class StaminaRegen : EntityStat
{
    public StaminaRegen(Vigor vig, Constitution con, Strength str) : base(new List<EntityAttribute> { vig, con, str })
    {
        vig.OnValueChanged += Recalculate;
        con.OnValueChanged += Recalculate;
        str.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float strength = _attributes.FirstOrDefault(attr => attr.Name == "Strength")?.Value ?? 0;
        return vigor * 0.5f + constitution * 0.4f + strength * 0.1f;
    }
}


public class StaminaRegenCD : EntityStat
{
    public StaminaRegenCD(Vigor vig, Constitution con, Strength str, Resistance res) : base(new List<EntityAttribute> { vig, con, str, res })
    {
        vig.OnValueChanged += Recalculate;
        con.OnValueChanged += Recalculate;
        str.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float strength = _attributes.FirstOrDefault(attr => attr.Name == "Strength")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        return 10 - (vigor * 2 + constitution + strength * 0.75f + resistance * 0.1f / 73);
    }
}   

public class MaxSpeed : EntityStat
{
    public MaxSpeed(Agility agi) : base(new List<EntityAttribute> { agi })
    {
        agi.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float agility = _attributes.FirstOrDefault(attr => attr.Name == "Agility")?.Value ?? 0;
        return 1 + 0.5f * agility;
    }
}   

public class Acceleration : EntityStat
{
    public Acceleration(Dextery dex) : base(new List<EntityAttribute> { dex })
    {
        dex.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float dextery = _attributes.FirstOrDefault(attr => attr.Name == "Dextery")?.Value ?? 0;
        return 0.05f * dextery;
    }
}   

public class AttackCD : EntityStat
{
    public AttackCD(Dextery dex, Agility agi, Finesse fin, Accuracy acc) : base(new List<EntityAttribute> { dex, agi, fin, acc })
    {
        dex.OnValueChanged += Recalculate;
        agi.OnValueChanged += Recalculate;
        fin.OnValueChanged += Recalculate;
        acc.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float dextery = _attributes.FirstOrDefault(attr => attr.Name == "Dextery")?.Value ?? 0;
        float agility = _attributes.FirstOrDefault(attr => attr.Name == "Agility")?.Value ?? 0;
        float finesse = _attributes.FirstOrDefault(attr => attr.Name == "Finesse")?.Value ?? 0;
        float accuracy = _attributes.FirstOrDefault(attr => attr.Name == "Accuracy")?.Value ?? 0;
        return 5 / 1 + (dextery * 12 / 100 + agility * 6 / 100 + finesse * 3 / 100 + accuracy * 1.5f / 100);
    }
}   

public class Precision : EntityStat
{
    public Precision(Accuracy acc) : base(new List<EntityAttribute> { acc })
    {
        acc.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        return _attributes.FirstOrDefault(attr => attr.Name == "Accuracy")?.Value ?? 0;
    }
}   

public class CriticalDamage : EntityStat
{
    public CriticalDamage(Finesse fin, Accuracy acc, Luck luck) : base(new List<EntityAttribute> { fin, acc, luck })
    {
        fin.OnValueChanged += Recalculate;
        acc.OnValueChanged += Recalculate;
        luck.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float finesse = _attributes.FirstOrDefault(attr => attr.Name == "Finesse")?.Value ?? 0;
        float accuracy = _attributes.FirstOrDefault(attr => attr.Name == "Accuracy")?.Value ?? 0;
        float luck = _attributes.FirstOrDefault(attr => attr.Name == "Luck")?.Value ?? 0;
        return 1.0f * 1 + (0.08f * finesse + 0.03f * accuracy + 0.01f * luck);
    }
}   

public class MagicDamage : EntityStat
{
    public MagicDamage(Intelligence intel) : base(new List<EntityAttribute> { intel })
    {
        intel.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float intelligence = _attributes.FirstOrDefault(attr => attr.Name == "Intelligence")?.Value ?? 0;
        return 10 * intelligence;
    }
}   

public class MagicDefense : EntityStat
{
    public MagicDefense(Willpower will, Spirit spr, Intelligence intel) : base(new List<EntityAttribute> { will, spr, intel })
    {
        will.OnValueChanged += Recalculate;
        spr.OnValueChanged += Recalculate;
        intel.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float willpower = _attributes.FirstOrDefault(attr => attr.Name == "Willpower")?.Value ?? 0;
        float spirit = _attributes.FirstOrDefault(attr => attr.Name == "Spirit")?.Value ?? 0;
        float intelligence = _attributes.FirstOrDefault(attr => attr.Name == "Intelligence")?.Value ?? 0;
        return 6 * willpower + (2 * spirit + intelligence) / 3;
    }
}   

public class MagicCD : EntityStat
{
    public MagicCD(Willpower will) : base(new List<EntityAttribute> { will })
    {
        will.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float willpower = _attributes.FirstOrDefault(attr => attr.Name == "Willpower")?.Value ?? 0;
        return willpower <= 1 ? 1 : Mathf.Pow(0.99f, willpower);
    }
}   

public class MaxMana : EntityStat
{
    public MaxMana(Spirit spr) : base(new List<EntityAttribute> { spr })
    {
        spr.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float spirit = _attributes.FirstOrDefault(attr => attr.Name == "Spirit")?.Value ?? 0;
        return 20 * spirit;
    }
}

public class Mana : ISubStat
{
    public IStat MainStat { get; private set; }
    public float CurrentValue { get; set; }

    public Mana(IStat mainStat)
    {
        MainStat = mainStat;
        CurrentValue = MainStat.CalculateStat();
    }
}

public class ManaRegen : EntityStat
{
    public ManaRegen(Intelligence intel, Spirit spr, Wisdom wis, Willpower will, Vigor vig, Constitution con, Resistance res)
        : base(new List<EntityAttribute> { intel, spr, wis, will, vig, con, res })
    {
        intel.OnValueChanged += Recalculate;
        spr.OnValueChanged += Recalculate;
        wis.OnValueChanged += Recalculate;
        will.OnValueChanged += Recalculate;
        vig.OnValueChanged += Recalculate;
        con.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float intelligence = _attributes.FirstOrDefault(attr => attr.Name == "Intelligence")?.Value ?? 0;
        float spirit = _attributes.FirstOrDefault(attr => attr.Name == "Spirit")?.Value ?? 0;
        float wisdom = _attributes.FirstOrDefault(attr => attr.Name == "Wisdom")?.Value ?? 0;
        float willpower = _attributes.FirstOrDefault(attr => attr.Name == "Willpower")?.Value ?? 0;
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        return intelligence * 0.1f + spirit * 0.35f + wisdom * 0.2f + willpower * 0.2f + vigor * 0.05f + constitution * 0.05f + resistance * 0.05f;
    }
}   

public class ManaRegenCD : EntityStat
{
    public ManaRegenCD(Intelligence intel, Spirit spr, Wisdom wis, Willpower will, Vigor vig, Constitution con, Resistance res)
        : base(new List<EntityAttribute> { intel, spr, wis, will, vig, con, res })
    {
        intel.OnValueChanged += Recalculate;
        spr.OnValueChanged += Recalculate;
        wis.OnValueChanged += Recalculate;
        will.OnValueChanged += Recalculate;
        vig.OnValueChanged += Recalculate;
        con.OnValueChanged += Recalculate;
        res.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float intelligence = _attributes.FirstOrDefault(attr => attr.Name == "Intelligence")?.Value ?? 0;
        float spirit = _attributes.FirstOrDefault(attr => attr.Name == "Spirit")?.Value ?? 0;
        float wisdom = _attributes.FirstOrDefault(attr => attr.Name == "Wisdom")?.Value ?? 0;
        float willpower = _attributes.FirstOrDefault(attr => attr.Name == "Willpower")?.Value ?? 0;
        float vigor = _attributes.FirstOrDefault(attr => attr.Name == "Vigor")?.Value ?? 0;
        float constitution = _attributes.FirstOrDefault(attr => attr.Name == "Constitution")?.Value ?? 0;
        float resistance = _attributes.FirstOrDefault(attr => attr.Name == "Resistance")?.Value ?? 0;
        return 20 - (intelligence * 3 + spirit * 2 + wisdom * 1.75f + willpower * 0.75f + vigor * 0.25f + constitution * 0.15f + resistance * 0.1f / 160);
    }
}   

public class DropRate : EntityStat
{
    public DropRate(Charisma cha, Luck luck, Presence pres) : base(new List<EntityAttribute> { cha, luck, pres })
    {
        cha.OnValueChanged += Recalculate;
        luck.OnValueChanged += Recalculate;
        pres.OnValueChanged += Recalculate;
    }


    public override float CalculateStat()
    {
        float charisma = _attributes.FirstOrDefault(attr => attr.Name == "Charisma")?.Value ?? 0;
        float luck = _attributes.FirstOrDefault(attr => attr.Name == "Luck")?.Value ?? 0;
        float presence = _attributes.FirstOrDefault(attr => attr.Name == "Presence")?.Value ?? 0;
        return 1 * 1 + (0.01f * charisma + 0.005f * luck + 0.00125f * presence);
    }
}   

public class SpawnRate : EntityStat
{
    public SpawnRate(Influence inf) : base(new List<EntityAttribute> { inf })
    {
        inf.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float influence = _attributes.FirstOrDefault(attr => attr.Name == "Influence")?.Value ?? 0;
        return 1 * 1 + (0.02f * influence);
    }
}   

public class TreasonRate : EntityStat
{
    public TreasonRate(Leadership lead) : base(new List<EntityAttribute> { lead })
    {
        lead.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float leadership = _attributes.FirstOrDefault(attr => attr.Name == "Leadership")?.Value ?? 0;
        return 1 * 1 + (0.01f * leadership);
    }
}   

public class TreasonDuration : EntityStat
{
    public TreasonDuration(Leadership lead, Influence inf, Charisma cha) : base(new List<EntityAttribute> { lead, inf, cha })
    {
        lead.OnValueChanged += Recalculate;
        inf.OnValueChanged += Recalculate;
        cha.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float leadership = _attributes.FirstOrDefault(attr => attr.Name == "Leadership")?.Value ?? 0;
        float influence = _attributes.FirstOrDefault(attr => attr.Name == "Influence")?.Value ?? 0;
        float charisma = _attributes.FirstOrDefault(attr => attr.Name == "Charisma")?.Value ?? 0;
        return 1 * 1 + (0.01f * leadership + 0.01f * influence + 0.01f * charisma);
    }
}   

public class SummonRate : EntityStat
{
    public SummonRate(Leadership lead, Presence pres) : base(new List<EntityAttribute> { lead, pres })
    {
        lead.OnValueChanged += Recalculate;
        pres.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float leadership = _attributes.FirstOrDefault(attr => attr.Name == "Leadership")?.Value ?? 0;
        float presence = _attributes.FirstOrDefault(attr => attr.Name == "Presence")?.Value ?? 0;
        return 1 * 1 + (0.02f * leadership + 0.02f * presence);
    }
}   

public class SummonDuration : EntityStat
{
    public SummonDuration(Leadership lead, Presence pres) : base(new List<EntityAttribute> { lead, pres })
    {
        lead.OnValueChanged += Recalculate;
        pres.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float leadership = _attributes.FirstOrDefault(attr => attr.Name == "Leadership")?.Value ?? 0;
        float presence = _attributes.FirstOrDefault(attr => attr.Name == "Presence")?.Value ?? 0;
        return 1 * 1 + (0.02f * leadership + 0.02f * presence);
    }
}   

public class SummonMorale : EntityStat
{
    public SummonMorale(Leadership lead, Presence pres) : base(new List<EntityAttribute> { lead, pres })
    {
        lead.OnValueChanged += Recalculate;
        pres.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float leadership = _attributes.FirstOrDefault(attr => attr.Name == "Leadership")?.Value ?? 0;
        float presence = _attributes.FirstOrDefault(attr => attr.Name == "Presence")?.Value ?? 0;
        return 1 * 1 + (0.02f * leadership + 0.02f * presence);
    }
}   

public class CriticalChance : EntityStat
{
    public CriticalChance(Luck luck) : base(new List<EntityAttribute> { luck })
    {
        luck.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float luck = _attributes.FirstOrDefault(attr => attr.Name == "Luck")?.Value ?? 0;
        return 0.01f + 0.01f * luck;
    }
}   

public class Experience : EntityStat
{
    public Experience(Level level) : base(new List<EntityAttribute> { level })
    {
        level.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        return 0;
    }
}   

public class NextLevelExp : EntityStat
{
    public NextLevelExp(List<EntityAttribute> attributes) : base(attributes) { }

    public override float CalculateStat()
    {
        float level = _attributes.FirstOrDefault(attr => attr.Name == "Level")?.Value ?? 0;
        if (level == 1 || level == 0)
        {
            return 100;
        }

        float nextLevelExp = Mathf.Ceil(100 * (1 + (1.03f * level / 10)));
        nextLevelExp *= Mathf.Max(1, Mathf.Floor(level / 10));
        if (level % 2 != 0)
        {
            nextLevelExp += level;
        }

        return nextLevelExp;
    }
}   

public class TotalAttPoints : EntityStat
{
    public TotalAttPoints(Level level) : base(new List<EntityAttribute> { level })
    {
        level.OnValueChanged += Recalculate;
    }

    public override float CalculateStat()
    {
        float level = _attributes.FirstOrDefault(attr => attr.Name == "Level")?.Value ?? 0;
        return 3 * level;
    }
}

public class SpentAttPoints : ISubStat
{
    public IStat MainStat { get; private set; } 
    public float CurrentValue { get; set; }

    public SpentAttPoints(TotalAttPoints mainStat)
    {
        MainStat = mainStat;
        CurrentValue = 0; 
    }

    public void IncreaseSpentPoints(int points)
    {
        CurrentValue += points;
    }
}

public class FreeAttPoints : ISubStat
{
    public IStat MainStat { get; private set; } // This will reference `TotalAttPoints`
    private SpentAttPoints spentAttPoints;
    public float CurrentValue { get; set; }

    public FreeAttPoints(IStat totalAttPoints, SpentAttPoints spentAttPoints)
    {
        MainStat = totalAttPoints;
        this.spentAttPoints = spentAttPoints;

        // Subscribe to changes in main stat and spent points
        if (MainStat is EntityStat entityStat)
        {
         //   entityStat.OnValueChanged += Recalculate;
        }
        if (spentAttPoints != null)
        {
         //   spentAttPoints.OnValueChanged += Recalculate;
        }

        CurrentValue = MainStat.CalculateStat() - spentAttPoints.CurrentValue; // Initial calculation
    }

    public void Recalculate(float newValue = 0)
    {
        CurrentValue = MainStat.CalculateStat() - spentAttPoints.CurrentValue;
    }
}



public class EntityStats : MonoBehaviour
{
    //attributes
    //physical
    [SerializeField] public EntityAttribute strenght;
    [SerializeField] public EntityAttribute resistance;
    [SerializeField] public EntityAttribute constitution;
    [SerializeField] public EntityAttribute vigor;
    //coordenação motora
    [SerializeField] public EntityAttribute agility;
    [SerializeField] public EntityAttribute dextery;
    [SerializeField] public EntityAttribute accuracy;
    [SerializeField] public EntityAttribute finesse;
    //mágico
    [SerializeField] public EntityAttribute intelligence;
    [SerializeField] public EntityAttribute willpower;
    [SerializeField] public EntityAttribute wisdom;
    [SerializeField] public EntityAttribute spirit;
    //social
    [SerializeField] public EntityAttribute charisma;
    [SerializeField] public EntityAttribute influence;
    [SerializeField] public EntityAttribute leadership;
    [SerializeField] public EntityAttribute presence;
    //special
    [SerializeField] public EntityAttribute luck; //afeta todos os stats, exceto quando explicito
    [SerializeField] public EntityAttribute level;
    [SerializeField] public EntityAttribute gold;


    //stats
    //physical
    [HideInInspector] public float physicalDamage;    // (strenght = 95%) (resistance = 5%)
    [HideInInspector] public float physicalDefense;   // 5 * resistance + (constitution+vigour)/2
    [HideInInspector] public float maxHealth;         // 100 + constitution * 25
    [HideInInspector] public float health;            // maxHealth
    [HideInInspector] public float healthRegen;       // constitution (50%) resistance (50%)
    [HideInInspector] public float healthRegenCD;     // once every 20 - (constitution * 4 + resistance * 3 + vigor * 2 / 90) seconds
    [HideInInspector] public float maxStamina;        // 100 + vigor * 10
    [HideInInspector] public float stamina;           // maxStamina
    [HideInInspector] public float staminaRegen;      // vigor (50%) constitution (40%) strenght (10%)
    [HideInInspector] public float staminaRegenCD;    // once every 10 - (vigor * 2 + constitution * 1 + strenght * 0.75 + resistance * 0.1 / 73 )
    //coordenação motora
    [HideInInspector] public float maxSpeed;          // 1 + 0.5 * agility
    [HideInInspector] public float acceleration;      // 0.05 * dextery
    [HideInInspector] public float decceleration;     // 0.5 / dextery 
    [HideInInspector] public float attackCD;          // 5/1 + (dextery*12/100 + agility*6/100 + finesse*3/100 + accuracy*1.5/100)
    [HideInInspector] public float precision;         // accuracy
    [HideInInspector] public float criticalDamage;    // 1.0 * 1 + (0.08 * finesse + 0.03 accuracy  + 0.01 luck)
    //mágico
    [HideInInspector] public float magicDamage;       // 10 * intelligence
    [HideInInspector] public float magicDefense;      // 6 * willpower (60%) + (2 * spirit + intelligence)/3
    [HideInInspector] public float cooldownReduction; // if <= 1 cooldownReduction = 1 else = 0.99 ^ willpower)
    [HideInInspector] public float maxMana;           // 20 * spirit
    [HideInInspector] public float mana;              // maxMana
    [HideInInspector] public float manaRegen;         // intelligence (10%) spirit (35%) wisdom (20%) willpower (20%) vigor (5%) constitution (5%) resistance (5%)
    [HideInInspector] public float manaRegenCD;       // once every 20 - (intelligence * 3 + spirit * 2 + wisdom * 1.75 + willpower * 0.75 + vigor * 0.25 + constitution * 0.15 + resistance * 0.1 / 160)
    //social
    [HideInInspector] public float dropRate;          // 1 * 1 + (0.01 * charisma + 0.005 LUCK + 0.00125 presence)
    [HideInInspector] public float spawnRate;         // 1 * 1 + (0.02 * influence)
    [HideInInspector] public float treasonRate;       // 1 * 1 + (0.01 * leadership) 
    [HideInInspector] public float treasonDuration;   // 1 * 1 + (0.01 * leadership + 0.01 influence + 0.01 charisma) 
    [HideInInspector] public float summonRate;        // 1 * 1 + (0.02 * leadership + 0.02 presence)
    [HideInInspector] public float summonDuration;    // 1 * 1 + (0.02 * leadership + 0.02 presence)
    [HideInInspector] public float summonMorale;      // 1 * 1 + (0.02 * leadership + 0.02 presence)
    //special
    [HideInInspector] public float criticalChance;    // 0.01 + (0.01 * luck)
    [HideInInspector] public float experience = 0;    // sempre zero, a não ser que o jogador ganhe derrotando inimigos
    [HideInInspector] public float nextLevelExp = 100;// if (level.value == 1 || level.value == 0)  {nextLevelExp = 100;return;} nextLevelExp = Mathf.Ceil(100 * (1 + (1.03f * level.value / 10)));nextLevelExp *= Mathf.Max(1, Mathf.Floor(level.value / 10));if (level.value % 2 != 0){nextLevelExp += level.value;}
    [HideInInspector] public int totalAttPoints = 0;// 3 * level
    [HideInInspector] public int spentAttPoints = 0;// aumenta conforme o jogador usa pontos livres para melhorar outros atributos
    [HideInInspector] public int freeAttPoints = 0; // totalAttPoints - spentAttPoints

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color darknedColor;

    public string[] attNames;
    public float[] attValues;

    void Start()
    {
        InitializeAttributes();
        InitiateStats();
    }

    void Update()
    {

    }

    

    void FixedUpdate()
    {
        CalculateMaxSpeed(true);
        CalculateAcceleration(true);
        CalculatePhysicalDamage(true);
        CheckLevelUp();
    }

    void InitializeAttributes()
    {
        List<string> namesList = new List<string>();
        List<float> valuesList = new List<float>();

        // Find all fields of type EntityAttribute
        FieldInfo[] fields = typeof(EntityStats).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(EntityAttribute))
            {
                EntityAttribute attribute = (EntityAttribute)field.GetValue(this);
                if (attribute == null)
                {
                    /*attribute = /*new EntityAttribute(); this comment was made because we are currently refactoring the code*/ // Ensure the attribute is not null
                    field.SetValue(this, attribute);
                }
                // Capitalize the first letter of the field name
                string fieldName = field.Name.Substring(0, 1).ToUpper() + field.Name.Substring(1);
                float initialValue = attribute.Value != 0 ? attribute.Value : 5;
                /*attribute.Initialize(fieldName, initialValue); this comment was made because we are currently refactoring the code*/ // Initialize with default value of 5

                // Add to lists
                namesList.Add(attribute.Name);
                valuesList.Add(attribute.Value);
            }
        }

        // Convert lists to arrays
        attNames = namesList.ToArray();
        attValues = valuesList.ToArray();
    }


    void InitiateStats()
    {

        maxHealth = 100 + constitution.Value * 25;
        maxStamina = 100 + vigor.Value * 10;
        maxMana = 100 + spirit.Value * 50;
        CalculateMaxSpeed(true);
        CalculateAcceleration(true);
        CalculatePhysicalDamage(true);
        health = maxHealth;
        stamina = maxStamina;
        mana = maxMana;
        experience = 0;
        nextLevelExp = 100;
        level.Value = 0;
        IncreaseLevel();
    }

    public float CalculatePhysicalDamage(bool update = false)
    {
        if (!update)
        {
            if (physicalDamage == 0)
            {
                CalculatePhysicalDamage(true);
            }
            return physicalDamage;
        }

        physicalDamage = (10 * strenght.Value);
        if (physicalDamage <= 0)
        {
            physicalDamage = 1f;
        }
        return physicalDamage;
    }

    public float CalculateMaxSpeed(bool update = false)
    {
        if (!update)
        {
            if (maxSpeed == 0)
            {
                CalculateMaxSpeed(true);
            }
            return maxSpeed;
        }

        maxSpeed = (10 + (agility.Value * 5)) / 10;
        if (maxSpeed <= 0)
        {
            maxSpeed = 0.001f;
        }
        return maxSpeed;
    }

    public float CalculateAcceleration(bool update = false)
    {
        if (!update)
        {
            if (acceleration == 0)
            {
                CalculateMaxSpeed(true);
            }
            return maxSpeed;
        }

        acceleration = (10f + dextery.Value) / 10f;
        if (acceleration <= 0)
        {
            acceleration = 0.001f;
        }
        return acceleration;
    }

    public void GainExperience(float incomingExperience)
    {
        experience += incomingExperience;
        if (experience >= nextLevelExp)
        {
            CheckLevelUp();
        }
    }

    void CheckLevelUp()
    {
        float tempExp = experience - nextLevelExp;
        if (experience > nextLevelExp)
        {
            experience = 0;
            IncreaseLevel();
            GainExperience(tempExp);
        }
        if (experience == nextLevelExp)
        {
            experience = 0;
            IncreaseLevel();
        }
    }

    void IncreaseLevel()
    {
        level.Value += 1;
        totalAttPoints = Mathf.FloorToInt(level.Value * 3);
        freeAttPoints = totalAttPoints - spentAttPoints;

        CalculateNextLevelExp();

        /* if (experience > nextLevelExp)
        {
            return;
        }
        else if (level.value != 1 || level.value != 0)
        {
            ShowLevelUpMenu();
        }*/
    }

    void CalculateNextLevelExp()
    {
        if (level.Value == 1 || level.Value == 0)
        {
            nextLevelExp = 100;
            return;
        }

        nextLevelExp = Mathf.Ceil(100 * (1 + (1.03f * level.Value / 10)));
        nextLevelExp *= Mathf.Max(1, Mathf.Floor(level.Value / 10));
        if (level.Value % 2 != 0)
        {
            nextLevelExp += level.Value;
        }
    }

    public int ReadLevel()
    {
        return Mathf.FloorToInt(level.Value);
    }

    public void IncreaseAttByName(string name, int value)
    {
        name = name.ToLower();
        FieldInfo field = typeof(EntityStats).GetField(name, BindingFlags.Public | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(EntityAttribute))
        {
            EntityAttribute attribute = (EntityAttribute)field.GetValue(this);
            if (attribute != null)
            {
                attribute.Value += value;
                spentAttPoints += value;
                UpdateAttPoints();
            }
        }
        else
        {
            Debug.LogWarning("Attribute with the name " + name + " not found or is not of type EntityAttribute.");
        }
    }

    void UpdateAttPoints()
    {
        freeAttPoints = totalAttPoints - spentAttPoints;
    }

    public int? ReadAttByName(string name)
    {
        name = name.ToLower();
        FieldInfo field = typeof(EntityStats).GetField(name, BindingFlags.Public | BindingFlags.Instance);
        if (field != null && field.FieldType == typeof(EntityAttribute))
        {
            EntityAttribute attribute = (EntityAttribute)field.GetValue(this);
            if (attribute != null)
            {
                return Mathf.FloorToInt(attribute.Value);
            }
        }
        else
        {
            Debug.LogWarning("Attribute with the name " + name + " not found or is not of type EntityAttribute.");
            return null;
        }
        return null;
    }


}   
