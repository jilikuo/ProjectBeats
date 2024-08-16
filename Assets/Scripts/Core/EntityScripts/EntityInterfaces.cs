using UnityEngine;

namespace Jili.StatSystem.EntityTree
{

    public interface IDamageable
    {
        public abstract bool TakeDamage(float incomingDamage);

        public abstract bool RunDeathRoutine();
    }

    public interface IHealable
    {
        public abstract bool HealDamage(float incomingHeal);

        public abstract bool Regeneration();
    }

    public interface IProjectile
    {
        public abstract bool TryStop(int strenght);
        public abstract void GetStopped();
        public abstract void VerifyRange();
        public abstract void OnCollisionEnter2D(Collision2D collision);
    }

    public interface IPlayer : IDamageable, IHealable
    {

    }   


}
