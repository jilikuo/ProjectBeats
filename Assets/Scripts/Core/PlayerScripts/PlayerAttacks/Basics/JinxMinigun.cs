using Jili.StatSystem.EntityTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{

    public class JinxMinigun : MonoBehaviour
    {
        protected readonly WeaponIDs thisID = WeaponIDs.JinxMinigun;
        protected readonly GameObject projectilePrefab = GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().Bullet;
        public GameObject player;
        public PlayerIdentity playerStats;
        public Transform playerTransform;
        private Vector3 shootPos;

        protected float shootRate = 2;
        protected float cooldown
        {
            get
            {
                if (isDirty)
                {
                    isDirty = false;
                    _cooldown = 1 / playerStats.AttacksPerSecond.Value;
                }
                return _cooldown;
            }
        }
        public float _cooldown;
        public float currentCD;
        public float projectileDuration = 10;
        public float offset = 0.05f;
        public float projectileSpeed = 20f;
        public float range = 2f;
        public float projectiles = 5;
        public float triggerSpeed;

        protected bool isDirty = true;

        protected JinxMinigun()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerStats = player.GetComponent<PlayerIdentity>();
            List<Stat> relevantStats = playerStats.statList.FindAll(x => (x.Type == StatType.AttackDamage)     ||
                                                                         (x.Type == StatType.AttacksPerSecond));
            playerTransform = player.transform;

            foreach (var stat in relevantStats)
            {
                stat.OnValueChanged += BecomeDirty;
            }

            _cooldown = 1 / playerStats.AttacksPerSecond.Value;
        }

        void BecomeDirty()
        {
            isDirty = true;
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
            shootPos = playerTransform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - shootPos).normalized;

            for (int i = 0; i < projectiles; i++)
            {
                shootPos = playerTransform.position;
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