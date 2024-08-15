/*using UnityEngine;

namespace Jili.OldStatSystem
{
    public interface IAttacker
    {
        //relevant attributes
        Strength strenght { get; set; }

        //relevant suicidalEntity
        PhysicalDamage physicalDamage { get; set; }
    }

    public interface IDamageable
    {
        //relevant attributes
        Constitution constitution { get; set; }

        //relevant suicidalEntity
        MaxHealth maxHealth { get; set; }
        Health health { get; set; }

        //relevant methods
        void TakeDamage(float incomingDamage);
    }

    public interface IDestructible : IDamageable
    {
        void CheckHealth();
        void StartDeathRoutine();
    }


    public interface IDamageReactive
    {
        SpriteRenderer spriteRenderer { get; set; }
        Color originalColor { get; set; }
        Color darknedColor { get; set; }


        void VisualizeDamage();
    }

    public interface ISuicidable : IAttacker, IDestructible, IDamageReactive
    {

        void Suicide();
    }


    public interface IMovable
    {
        //relevant attributes
        Agility agility { get; set; }

        //relevant suicidalEntity
        MaxSpeed maxSpeed { get; set; }
    }

    public interface IAccerable
    {
        //relevant attributes
        Dextery dextery { get; set; }

        //relevant suicidalEntity
        Acceleration acceleration { get; set; }
    }

    public interface IExperience
    {
        //relevant attributes
        Level level { get; set; }

        //relevant suicidalEntity
        Experience experience { get; set; }
        NextLevelExp nextLevelExperience { get; set; }

        //relevant methods
        void GainExperience(float incomingExperience);
        void CheckLevelUp();
        void IncreaseLevel();
        int ReadLevel();
    }



    public class EntityInterfaces : MonoBehaviour
    {

    }
}*/