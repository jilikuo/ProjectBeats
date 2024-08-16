/*
//TODO: refazer essa classe do zero, para utilizar uma nova lógica de projéteis;
// Porém ainda talvez seja utilizada, deve ser analizado com calma antes de tomar uma decisão final;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject parent;
    public GameObject target;
    public readonly string playerTag = "Player";
    private Vector3 startingPoint; 
    private float damage;
    public float range;

    void Start()
    {
        // damage = parent.GetComponent<Attribute>().CalculatePhysicalDamage() * 0.50f;
        // Range = parent.GetComponent<PistolShoot>().Range;
        startingPoint = parent.transform.position;
    }

    void FixedUpdate()
    {
       VerifyOutOfRange();
    }

    void VerifyOutOfRange()
    {
        float distanceTravelled = Vector3.Distance(startingPoint, transform.position);
        if (distanceTravelled > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Consumable"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            if (collision.gameObject.GetComponent<Projectile>().parent == parent)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                return;
            }
            else
            {
                DestroyImmediate(collision.gameObject);
                DestroyImmediate(gameObject);
                return;
            }
        }
        if (collision.gameObject.CompareTag(parent.tag))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            return;
        }
        if (parent.CompareTag(playerTag))
        {
            // collision.gameObject.GetComponent<Attribute>().TakeDamage(damage);
            //Destroy(this.gameObject);
            return;
        }
    }
}
*/