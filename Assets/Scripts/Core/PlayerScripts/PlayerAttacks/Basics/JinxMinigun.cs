using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{

    public class JinxMinigun : MonoBehaviour
    {
        public WeaponIDs ID = WeaponIDs.JinxMinigun;
        public GameObject projectilePrefab;
        public Transform shooterPos;
        private Vector3 shootPos;


        public float cooldown = 2f;
        public float currentCD = 0f;
        public float projectileDuration = 10f;
        public float offset = 0.05f;
        public float projectileSpeed = 20f;
        public float range = 2f;
        public float projectiles = 5;
        public float triggerSpeed;

        void Start()
        {
            shooterPos = GameObject.FindGameObjectWithTag("Player").transform;
            triggerSpeed = (cooldown * 0.20f) / projectiles;
        }

        void FixedUpdate()
        {
            if (CoolDownManager())
            {
                StartCoroutine(ShootProjectiles());
            }
        }

        bool CoolDownManager()
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

        IEnumerator ShootProjectiles()
        {
            shootPos = shooterPos.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - shootPos).normalized;

            for (int i = 0; i < projectiles; i++)
            {
                shootPos = shooterPos.position;
                Shoot(direction);
                yield return new WaitForSeconds(triggerSpeed);
            }
        }

        void Shoot(Vector2 direction)
        {
            // Generate random offset
            float offsetX = Random.Range(-offset, offset);
            float offsetY = Random.Range(-offset, offset);
            Vector3 spawnPosition = shootPos + new Vector3(offsetX, offsetY, 0);

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
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
}