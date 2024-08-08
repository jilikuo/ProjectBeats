using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

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
    [HideInInspector] public float criticalChance;    // luck
    [HideInInspector] public float experience = 0;    // exp
    [HideInInspector] public float nextLevelExp = 100;// exp
    [HideInInspector] public int totalAttPoints = 0;// level
    [HideInInspector] public int spentAttPoints = 0;// level
    [HideInInspector] public int freeAttPoints = 0; // level

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color darknedColor;

    public string[] attNames;
    public float[] attValues;

    void Awake()
    {

    }

    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        darknedColor = originalColor * 0.25f;
        InitializeAttributes();
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
                    attribute = new EntityAttribute(); // Ensure the attribute is not null
                    field.SetValue(this, attribute);
                }
                // Capitalize the first letter of the field name
                string fieldName = field.Name.Substring(0, 1).ToUpper() + field.Name.Substring(1);
                float initialValue = attribute.value != 0 ? attribute.value : 5;
                attribute.Initialize(fieldName, initialValue); // Initialize with default value of 5

                // Add to lists
                namesList.Add(attribute.Name);
                valuesList.Add(attribute.value);
            }
        }

        // Convert lists to arrays
        attNames = namesList.ToArray();
        attValues = valuesList.ToArray();
    }


    void InitiateStats()
    {

        maxHealth = 100 + constitution.value * 25;
        maxStamina = 100 + vigor.value * 10;
        maxMana = 100 + spirit.value * 50;
        CalculateMaxSpeed(true);
        CalculateAcceleration(true);
        CalculatePhysicalDamage(true);
        health = maxHealth;
        stamina = maxStamina;
        mana = maxMana;
        experience = 0;
        nextLevelExp = 100;
        level.value = 0;
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

        physicalDamage = (10 * strenght.value);
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

        maxSpeed = (10 + (agility.value * 5)) / 10;
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

        acceleration = (10f + dextery.value) / 10f;
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
        if (gameObject.CompareTag("Enemy"))
        {
            gameObject.GetComponent<EnemyDrop>().CheckDrop();
        }
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
        level.value += 1;
        totalAttPoints = Mathf.FloorToInt(level.value * 3);
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
        if (level.value == 1 || level.value == 0)
        {
            nextLevelExp = 100;
            return;
        }

        nextLevelExp = Mathf.Ceil(100 * (1 + (1.03f * level.value / 10)));
        nextLevelExp *= Mathf.Max(1, Mathf.Floor(level.value / 10));
        if (level.value % 2 != 0)
        {
            nextLevelExp += level.value;
        }
    }

    public int ReadLevel()
    {
        return Mathf.FloorToInt(level.value);
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
                attribute.value += value;
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
                return Mathf.FloorToInt(attribute.value);
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

[System.Serializable]   
public class EntityAttribute
{
    [SerializeField, HideInInspector] public string Name;
    [SerializeField] public float value;

    public void Initialize(string name, float initialvalue)
    {
        Name = name;
        value = initialvalue;
    }
}