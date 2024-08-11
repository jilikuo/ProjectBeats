using UnityEngine;
using Jili.StatSystem;

[RequireComponent(typeof(CollisionSuicidalEnemy))]
public class EnemyMoveSuicidal : MonoBehaviour
{
    private GameObject target;
    private Transform playerPos;

    private EntityBase stats;
    private Rigidbody2D enemyRb;
    private float speed;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        stats = GetComponent<EntityBase>();
        enemyRb = GetComponent<Rigidbody2D>();

        playerPos = target.transform;

        speed = stats.ReadStatValueByType(StatType.MovementSpeed);
    }

    void FixedUpdate()
    {
        Vector3 direction = (playerPos.position - transform.position).normalized;

        enemyRb.velocity = direction * speed;
    }
}
