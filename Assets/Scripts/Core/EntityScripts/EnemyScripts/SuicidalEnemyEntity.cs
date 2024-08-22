using UnityEngine;

namespace Jili.StatSystem.EntityTree
{
    public enum SuicidalEnemyType
    {
        Small,
        Big,
        Boss
    }

    [RequireComponent(typeof(EnemyMoveSuicidal))]
    public class SuicidalEnemyEntity : EntityBase, IDamageable, IShowDamage
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

        private Color originalColor;
        private Color darknedColor;

        public SuicidalEnemyType type;

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

            originalColor = gameObject.GetComponent<SpriteRenderer>().color;
            darknedColor = originalColor * 0.65f;
            darknedColor.a = 1;
        }

        public bool TakeDamage(float incomingDamage)
        {
            Health.CurrentVolatileValue -= incomingDamage;
            UpdateColorBasedOnRemainingHP();
            if (Health.CurrentVolatileValue <= 0)
            {
                RunDeathRoutine();
                return true;
            }
            return false;
        }

        public void UpdateColorBasedOnRemainingHP()
        {
            float healthPercentage = Health.CurrentVolatileValue / Health.Value;
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(darknedColor, originalColor, healthPercentage);
        }

        public bool RunDeathRoutine()
        {
            gameObject.GetComponent<EnemyDrop>().StartDropRoutine();
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