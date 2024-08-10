/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem;

public class CollisionSuicidalEnemy : MonoBehaviour
{
    public GameObject target;
    private EntityStats stats;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<EntityStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            EntityStats targetStats = collision.gameObject.GetComponent<EntityStats>();
           /* targetStats.TakeDamage(stats.CalculatePhysicalDamage());
            stats.Suicide(); 
        }
    }
}
*/