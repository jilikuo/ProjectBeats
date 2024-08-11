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

    public interface IPlayer : IDamageable, IHealable
    {

    }   


}
