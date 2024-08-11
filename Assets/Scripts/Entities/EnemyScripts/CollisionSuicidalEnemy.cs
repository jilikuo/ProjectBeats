/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem;

public class CollisionSuicidalEnemy : MonoBehaviour
{
    public GameObject target;
    private EnemyBasicBomb stats;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<EnemyBasicBomb>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            EnemyBasicBomb targetStats = collision.gameObject.GetComponent<EnemyBasicBomb>();
            //targetStats.TakeDamage(stats.CalculatePhysicalDamage());
            stats.Suicide();
        }
    }
}
*/