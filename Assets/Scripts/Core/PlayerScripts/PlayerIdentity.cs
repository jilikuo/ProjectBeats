using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem.AttackSystem;
using System.Linq;
using System;


namespace Jili.StatSystem.EntityTree
{

    public class PlayerIdentity : EntityBase, IPlayer
    {
        public float str;                       // PHYSICAL
        public float con;
        public float vig;
        public float dex;                       // MOBILITY
        public float agi;
        public float fin;
        public float pre;
        public float will;                      // MAGICAL
        public float extraProjectilesNumber;    // INDEPENDENT
        public float extraProjectilesSpeed;

        public Attribute Strength;              // PHYSICAL
        public Attribute Constitution;
        public Attribute Vigor;
        public Attribute Dextery;               // MOBILITY
        public Attribute Agility;
        public Attribute Finesse;
        public Attribute Precision;
        public Attribute Willpower;             // MAGICAL
        public Stat AttackDamage;
        public Stat Health;
        public Stat HealthRegen;
        public Stat Mana;
        public Stat MovementSpeed;
        public Stat Acceleration;
        public Stat AttacksPerSecond;
        public Stat AttackRange;
        public Stat ProjectileNumber;
        public Stat ProjectileSpeed;

        private bool canHeal = true;       // flag for status conditions that block healing
        private float maxRegenCooldown = 1;   // (1 segundo)
        private float regenCooldown = 1;

        public IShootable baseWeapon;

        public static event Action OnPlayerDeath;

        void Awake()
        {
            // Start Attributes
            Strength = new Attribute(AttributeType.Strength, str);
            Constitution = new Attribute(AttributeType.Constitution, con);
            Vigor = new Attribute(AttributeType.Vigor, vig);
            Dextery = new Attribute(AttributeType.Dextery, dex);
            Agility = new Attribute(AttributeType.Agility, agi);
            Finesse = new Attribute(AttributeType.Finesse, fin);
            Precision = new Attribute(AttributeType.Precision, pre);
            Willpower = new Attribute(AttributeType.Willpower, will);

            // Load Entity Attribute List
            attListAdd(Strength);
            attListAdd(Constitution);
            attListAdd(Vigor);
            attListAdd(Dextery);
            attListAdd(Agility);
            attListAdd(Finesse);
            attListAdd(Precision);
            attListAdd(Willpower);

            // Start Stats
            AttackDamage = new Stat(StatType.AttackDamage, attList);
            Health = new Stat(StatType.Health, attList);
            HealthRegen = new Stat(StatType.HealthRegen, attList); 
            Mana = new Stat(StatType.Mana, attList);
            MovementSpeed = new Stat(StatType.MovementSpeed, attList);
            Acceleration = new Stat(StatType.Acceleration, attList);
            AttacksPerSecond = new(StatType.AttacksPerSecond, attList);
            AttackRange = new(StatType.AttackRange, attList);
            ProjectileNumber = new(StatType.ProjectileNumber, extraProjectilesNumber);
            ProjectileSpeed = new(StatType.ProjectileSpeed, extraProjectilesSpeed);

            // Load Entity Stat List
            statListAdd(AttackDamage);
            statListAdd(Health);
            statListAdd(HealthRegen);
            statListAdd(Mana);
            statListAdd(MovementSpeed);
            statListAdd(Acceleration);
            statListAdd(AttacksPerSecond);
            statListAdd(AttackRange);
            statListAdd(ProjectileNumber);
            statListAdd(ProjectileSpeed);

            baseWeapon = new JinxMinigun(this);
        }

        void Update()
        {
            TryRegenerate();
        }

        public void EquipNewCard(ScriptableCardData cardInfo)
        {
            Type scriptType = cardInfo.cardObject.GetClass();

            if (typeof(IShootable).IsAssignableFrom(scriptType))
            {
                IShootable newWeapon = (IShootable)System.Activator.CreateInstance(scriptType);
                this.gameObject.GetComponent<PlayerAttacks>().EquipWeapon(newWeapon);
            }
            else
            {
                Debug.Log("Card Object is not a weapon, it is " + cardInfo.cardObject.GetType());
            }
        }

        public bool TakeDamage(float incomingDamage)
        {
            Health.CurrentVolatileValue -= incomingDamage;
            if (Health.CurrentVolatileValue <= 0)
            {
                RunDeathRoutine();
                return true;
            }
            return false;
        }

        public bool HealDamage(float incomingHeal)
        {
            if (Health.CurrentVolatileValue == Health.Value || canHeal == false)
            {
                return false;
            }
            else
            {
                if ((Health.CurrentVolatileValue + incomingHeal) > Health.Value)
                {
                    Health.CurrentVolatileValue = Health.Value;
                    return true;
                }
                else
                {
                    Health.CurrentVolatileValue += incomingHeal;
                    return true;
                }
            }
        }

        public bool Regeneration()
        {
            float regen = HealthRegen.Value;
            if (HealDamage(regen))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TryRegenerate()
        {
            if (regenCooldown <= 0)
            {
                regenCooldown = maxRegenCooldown;
                Regeneration();
            }
            else
            {
                regenCooldown -= Time.deltaTime;
            }
        }

        public bool RunDeathRoutine()
        {
            Debug.Log("Player is Dead");
            this.gameObject.GetComponent<PlayerControl>().enabled = false;
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            this.gameObject.GetComponent<PlayerAttacks>().enabled = false;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            OnPlayerDeath?.Invoke();
            return true;
        }

        public override float ReadStatValueByType(StatType type)
        {
            float value = statList.Find(stat => stat.Type == type).Value;
            return value;
        }
    }
}