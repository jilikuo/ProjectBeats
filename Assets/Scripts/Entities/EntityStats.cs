using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    //attributes
    //physical
    public float strenght;
    public float resistance;
    public float constitution;
    public float vigor;
    //coordena��o motora
    public float agility;
    public float dextery;
    public float accuracy;
    public float finesse;
    //m�gico
    public float intelligence;
    public float willpower;
    public float wisdom;
    public float spirit;
    //social
    public float charisma;
    public float influence;
    public float leadership;
    public float presence;
    //special
    public float luck; //afeta todos os stats, exceto quando explicito

    //stats
    //physical
    private float physicalDamage;    // strenght (95%) resistance (5%)
    private float physicalDefense;   // resistance
    private float health;            // constitution
    private float maxHealth;         // constitution
    private float healthRegen;       // constitution (50%) resistance (50%)
    private float stamina;           // vigor
    private float maxStamina;        // vigor
    private float staminaRegen;      // vigor (50%) constitution (25%) strenght (20%) resistance (5%)
    //coordena��o motora
    private float maxSpeed;          // agility
    private float acceleration;      // dextery
    private float decceleration;     // dextery?
    private float attackPerSecond;   // dextery 50% agility 40% finesse 10%
    private float precision;         // accuracy
    private float criticalDamage;    // finesse (80%) accuracy (15%) luck (5%)
    //m�gico
    private float magicDamage;       // intelligence
    private float magicDefense;      // willpower (60%) spirit (15%) intelligence (15%)
    private float cooldownReduction; // wisdom
    private float mana;              // spirit
    private float maxMana;           // spirit
    private float manaRegen;         // intelligence (10%) spirit (35%) wisdom (20%) willpower (20%) vigor (5%) constitution (5%) resistance (5%)
    //social
    private float dropRate;          // charisma luck
    private float spawnRate;         // influence
    private float treasonRate;       // leadership luck 
    private float treasonDuration;   // leadership influence
    private float summonRate;        // leadership (80%) charisma (20%) 
    private float summonDuration;    // presence
    private float summonMorale;      // presence
    //special
    private float criticalChance;    //luck

    void Start()
    {
        InitiateStats();
    }

    private void Update()
    {
        CheckHealth();
    }

    void FixedUpdate()
    {
        CalculateMaxSpeed(true);
        CalculateAcceleration(true);
        CalculatePhysicalDamage(true);
    }



    void InitiateStats()
    {
        maxHealth = 100 + constitution * 25;
        maxStamina = 100 + vigor * 10;
        maxMana = 100 + spirit * 50;
        CalculateMaxSpeed(true);
        CalculateAcceleration(true);
        CalculatePhysicalDamage(true);
        health = maxHealth;
        stamina = maxStamina;
        mana = maxMana;
    }

    public float CalculatePhysicalDamage(bool update = false)
    {
        if (!update)
        {
            return physicalDamage;
        }

        physicalDamage = (10 * strenght);
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

        maxSpeed = (10 + (agility * 5))/10;
        if (maxSpeed == 0)
        {
            maxSpeed = 0.001f;
        }
        return maxSpeed;
    }

    public float CalculateAcceleration(bool update = false)
    {
        if (!update)
        {
            return acceleration;
        }

        acceleration = (10f + dextery)/10f;
        return acceleration;
    }

    public void Suicide()
    {
        health = 0;
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        } 
    }

    public void TakeDamage(float incomingDamage)
    {
        health -= incomingDamage;
    }
}
