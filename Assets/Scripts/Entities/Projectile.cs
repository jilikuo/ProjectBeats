using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject parent;
    public GameObject target;
    public readonly string playerTag = "Player"; 
    private float damage;

    void Start()
    {
        damage = parent.GetComponent<EntityStats>().CalculatePhysicalDamage();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(parent.tag))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
        if (parent.CompareTag(playerTag))
        {
            collision.gameObject.GetComponent<EntityStats>().TakeDamage(damage);
            Destroy(this.gameObject);
            return;
        }
    }
}
