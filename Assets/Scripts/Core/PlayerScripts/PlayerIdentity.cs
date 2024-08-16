using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem.AttackSystem;


namespace Jili.StatSystem.EntityTree
{

    public class PlayerIdentity : EntityBase, IPlayer
    {
        public float str;
        public float con;
        public float dex;
        public float agi;

        public Attribute Strength;
        public Attribute Constitution;
        public Attribute Dextery;
        public Attribute Agility;
        public Stat AttackDamage;
        public Stat Health;
        public Stat MovementSpeed;
        public Stat Acceleration;
        public Stat AttacksPerSecond;

        private bool healBlock = false; // flag for status conditions that block healing
        public IShootable baseWeapon;

        void Awake()
        {
            // Start Attributes
            Strength = new Attribute(AttributeType.Strength, str);
            Constitution = new Attribute(AttributeType.Constitution, con);
            Dextery = new Attribute(AttributeType.Dextery, dex);
            Agility = new Attribute(AttributeType.Agility, agi);

            // Load Entity Attribute List
            attListAdd(Strength);
            attListAdd(Constitution);
            attListAdd(Dextery);
            attListAdd(Agility);

            // Start Stats
            AttackDamage = new Stat(StatType.AttackDamage, attList);
            Health = new Stat(StatType.Health, attList);
            MovementSpeed = new Stat(StatType.MovementSpeed, attList);
            Acceleration = new Stat(StatType.Acceleration, attList);
            AttacksPerSecond = new(StatType.AttacksPerSecond, attList);

            // Load Entity Stat List
            statListAdd(AttackDamage);
            statListAdd(Health);
            statListAdd(MovementSpeed);
            statListAdd(Acceleration);
            statListAdd(AttacksPerSecond);

            baseWeapon = new JinxMinigun(5, 5, 5, 5, 5, GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().Bullet, this.gameObject);
        }

        void Update()
        {

            // Regeneration();
        }

        public bool TakeDamage(float incomingDamage)
        {
            Health.CurrentValue -= incomingDamage;
            if (Health.CurrentValue <= 0)
            {
                RunDeathRoutine();
                return true;
            }
            return false;
        }

        public bool HealDamage(float incomingHeal)
        {
            if (Health.CurrentValue == Health.Value || healBlock == true)
            {
                return false;
            }
            else
            {
                if ((Health.CurrentValue + incomingHeal) > Health.Value)
                {
                    Health.CurrentValue = Health.Value;
                    return true;
                }
                else
                {
                    Health.CurrentValue += incomingHeal;
                    return true;
                }
            }
        }

        public bool Regeneration()
        {
            float regen = 1;

            // TODO: calculate regen Cooldown based on attributes and suicidalEntity,
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