using UnityEngine;

namespace Jili.StatSystem.EntityTree
{
    [RequireComponent(typeof(EnemyMoveSuicidal))]
    public class SuicidalEnemyEntity : EntityBase, IDamageable
    {
        public float str;
        public float con;
        public float dex;

        private Attribute Strength;
        private Attribute Constitution;
        private Attribute Dextery;

        private Stat AttackDamage;
        private Stat Health;
        private Stat MovementSpeed;

        void Awake()
        {
            Strength = new Attribute(AttributeType.Strength, str);
            Constitution = new Attribute(AttributeType.Constitution, con);
            Dextery = new Attribute(AttributeType.Dextery, dex);

            attListAdd(Strength);
            attListAdd(Constitution);
            attListAdd(Dextery);

            AttackDamage = new Stat(StatType.AttackDamage, attList);
            Health = new Stat(StatType.Health, attList);
            MovementSpeed = new Stat(StatType.MovementSpeed, attList);

            statListAdd(AttackDamage);
            statListAdd(Health);
            statListAdd(MovementSpeed);
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

        public bool RunDeathRoutine()
        {
            Destroy(this.gameObject);
            return true;
        }

        public void Suicide()
        {
            RunDeathRoutine();
        }

        public override float ReadStatValueByType(StatType type)
        {
            float value = statList.Find(stat => stat.Type == type).Value;
            return value;
        }
    }
}