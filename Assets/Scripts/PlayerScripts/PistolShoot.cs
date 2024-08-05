using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform playerPos;
    public float cooldown = 2f;
    public float currentCD = 0f;
    public float projectileDuration = 10f;
    public float offset = 0.5f;
    public float projectileSpeed = 2f;

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    void FixedUpdate()
    {
        if (CanShoot())
        {
            Shoot();
        }    
    }

    bool CanShoot()
    {
        currentCD -= Time.fixedDeltaTime;
        if (currentCD < 0)
        {
            currentCD = 0;
        } 

        if (currentCD <= 0)
        {
            currentCD = cooldown;
            return true;
        }
        return false;
    }

    void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 direction = (mousePosition - playerPos.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, playerPos.position, Quaternion.identity);
        Projectile script = projectile.GetComponent<Projectile>();
        if (script != null)
        {
            script.parent = gameObject;
            Collider2D parentCollider = gameObject.GetComponent<Collider2D>();
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            if (parentCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(projectileCollider, parentCollider);
            }
        }

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;
        Destroy(projectile, projectileDuration);
    }
}
