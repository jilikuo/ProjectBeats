using UnityEngine;
using Jili.StatSystem;
using Jili.StatSystem.EntityTree;

public class CollisionSuicidalEnemy : MonoBehaviour
{
    public GameObject target;
    private SuicidalEnemyEntity suicidalEntity;
    float damage;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        suicidalEntity = GetComponent<SuicidalEnemyEntity>();
        damage = suicidalEntity.ReadStatValueByType(StatType.AttackDamage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            PlayerIdentity targetStats = collision.gameObject.GetComponent<PlayerIdentity>();
            targetStats.TakeDamage(damage);
            suicidalEntity.Suicide();
        }
    }
}