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
    //coordenação motora
    public float agility;
    public float dextery;
    public float accuracy;
    public float finesse;
    //mágico
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
    [HideInInspector] public float physicalDamage;    // strenght (95%) resistance (5%)
    [HideInInspector] public float physicalDefense;   // resistance
    [HideInInspector] public float health;            // constitution
    [HideInInspector] public float maxHealth;         // constitution
    [HideInInspector] public float healthRegen;       // constitution (50%) resistance (50%)
    [HideInInspector] public float stamina;           // vigor
    [HideInInspector] public float maxStamina;        // vigor
    [HideInInspector] public float staminaRegen;      // vigor (50%) constitution (25%) strenght (20%) resistance (5%)
    //coordenação motora
    [HideInInspector] public float maxSpeed;          // agility
    [HideInInspector] public float acceleration;      // dextery
    [HideInInspector] public float decceleration;     // dextery?
    [HideInInspector] public float attackPerSecond;   // dextery 50% agility 40% finesse 10%
    [HideInInspector] public float precision;         // accuracy
    [HideInInspector] public float criticalDamage;    // finesse (80%) accuracy (15%) luck (5%)
    //mágico
    [HideInInspector] public float magicDamage;       // intelligence
    [HideInInspector] public float magicDefense;      // willpower (60%) spirit (15%) intelligence (15%)
    [HideInInspector] public float cooldownReduction; // wisdom
    [HideInInspector] public float mana;              // spirit
    [HideInInspector] public float maxMana;           // spirit
    [HideInInspector] public float manaRegen;         // intelligence (10%) spirit (35%) wisdom (20%) willpower (20%) vigor (5%) constitution (5%) resistance (5%)
    //social
    [HideInInspector] public float dropRate;          // charisma luck
    [HideInInspector] public float spawnRate;         // influence
    [HideInInspector] public float treasonRate;       // leadership luck 
    [HideInInspector] public float treasonDuration;   // leadership influence
    [HideInInspector] public float summonRate;        // leadership (80%) charisma (20%) 
    [HideInInspector] public float summonDuration;    // presence
    [HideInInspector] public float summonMorale;      // presence
    //special
    [HideInInspector] public float criticalChance;    //luck

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color darknedColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        darknedColor = originalColor * 0.25f;
        InitiateStats();
    }

    void Update()
    {
        CheckHealth();
        VisualizeDamage();
    }

    void VisualizeDamage()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            float healthPercentage = health / maxHealth;

            // Update opacity
            Color newColor = Color.Lerp(darknedColor, originalColor, healthPercentage);
            newColor.a = 1;
            spriteRenderer.color = newColor;
        }
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
            if (physicalDamage == 0)
            {
                CalculatePhysicalDamage(true);
            }
            return physicalDamage;
        }

        physicalDamage = (10 * strenght);
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

        maxSpeed = (10 + (agility * 5)) / 10;
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

        acceleration = (10f + dextery) / 10f;
        if (acceleration <= 0)
        {
            acceleration = 0.001f;
        }
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
            StartDeathRoutine();
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        health -= incomingDamage;
    }

    void StartDeathRoutine()
    {
        
    }
}
