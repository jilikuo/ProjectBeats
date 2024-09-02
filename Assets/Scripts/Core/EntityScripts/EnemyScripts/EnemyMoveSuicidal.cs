    using UnityEngine;
using Jili.StatSystem;

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

        // Rotaciona o inimigo para olhar na direção do movimento, ajustando para a sprite que olha para baixo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Como a sprite olha para baixo, ajustamos o ângulo adicionando 90 graus
        enemyRb.rotation = angle + 90f;
    }
}

