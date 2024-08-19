using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem.AttackSystem;
using System.Linq;


namespace Jili.StatSystem.EntityTree
{

    public class PlayerIdentity : EntityBase, IPlayer
    {
        public float str;                       // PHYSICAL
        public float con;
        public float dex;                       // MOBILITY
        public float agi;
        public float fin;
        public float pre;
        public float will;                      // MAGICAL
        public float extraProjectilesNumber;    // INDEPENDENT
        public float extraProjectilesSpeed;

        public Attribute Strength;              // PHYSICAL
        public Attribute Constitution;
        public Attribute Dextery;               // MOBILITY
        public Attribute Agility;
        public Attribute Finesse;
        public Attribute Precision;
        public Attribute Willpower;             // MAGICAL
        public Stat AttackDamage;
        public Stat Health;
        public Stat Mana;
        public Stat MovementSpeed;
        public Stat Acceleration;
        public Stat AttacksPerSecond;
        public Stat AttackRange;
        public Stat ProjectileNumber;
        public Stat ProjectileSpeed;

        private bool healBlock = false; // flag for status conditions that block healing
        public IShootable baseWeapon;

        void Awake()
        {
            // Start Attributes
            Strength = new Attribute(AttributeType.Strength, str);
            Constitution = new Attribute(AttributeType.Constitution, con);
            Dextery = new Attribute(AttributeType.Dextery, dex);
            Agility = new Attribute(AttributeType.Agility, agi);
            Finesse = new Attribute(AttributeType.Finesse, fin);
            Precision = new Attribute(AttributeType.Precision, pre);
            Willpower = new Attribute(AttributeType.Willpower, will);

            // Load Entity Attribute List
            attListAdd(Strength);
            attListAdd(Constitution);
            attListAdd(Dextery);
            attListAdd(Agility);
            attListAdd(Finesse);
            attListAdd(Precision);
            attListAdd(Willpower);

            // Start Stats
            AttackDamage = new Stat(StatType.AttackDamage, attList);
            Health = new Stat(StatType.Health, attList);
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
            statListAdd(Mana);
            statListAdd(MovementSpeed);
            statListAdd(Acceleration);
            statListAdd(AttacksPerSecond);
            statListAdd(AttackRange);
            statListAdd(ProjectileNumber);
            statListAdd(ProjectileSpeed);

            baseWeapon = new JinxMinigun(GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().Bullet, this.gameObject);
        }

        void Update()
        {

            // TODO: Regeneration();
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
            if (Health.CurrentVolatileValue == Health.Value || healBlock == true)
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
            float regen = 1;

            // TODO: calculate regen Cooldown based on attributes and player stats,
            // if can regen (Cooldown <= 0) then regen.
            // if can't regen (in other words, healdamage() returns false),
            // then this return false


            if (HealDamage(regen))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RunDeathRoutine()
        {
            Debug.Log("Player is Dead");
            return true;
        }

        public override float ReadStatValueByType(StatType type)
        {
            float value = statList.Find(stat => stat.Type == type).Value;
            return value;
        }
    }
}