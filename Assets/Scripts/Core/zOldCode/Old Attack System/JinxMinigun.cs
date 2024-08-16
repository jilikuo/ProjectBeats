/*using Jili.StatSystem.EntityTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem.Old
{

    public class JinxMinigun : MonoBehaviour
    {
        public WeaponIDs thisID = WeaponIDs.JinxMinigun;
        public GameObject projectilePrefab; 
        public GameObject player;
        public PlayerIdentity playerStats;
        public Transform playerTransform;
        private Vector3 shootPos;

        protected float shootRate = 2;
        protected float Cooldown
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
        public float Range = 2f;
        public float projectiles = 5;
        public float triggerSpeed;

        protected bool isDirty = true;

        public JinxMinigun()
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

        private void Awake()
        {
            projectilePrefab = GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().Bullet;
        }

        private void FixedUpdate()
        {
            float deltatime = Time.fixedDeltaTime;
            if (CoolDownManager(deltatime))
            {
                StartCoroutine(ShootProjectiles());
            }
        }

        void BecomeDirty()
        {
            isDirty = true;
        }

        public bool CoolDownManager(float deltatime)
        {
            currentCD -= deltatime;
            if (currentCD < 0)
            {
                currentCD = 0;
            }

            if (currentCD <= 0)
            {
                currentCD = Cooldown;
                return true;
            }
            return false;
        }

        public IEnumerator ShootProjectiles()
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

            GameObject Projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Projectile script = Projectile.GetComponent<Projectile>();
            if (script != null)
            {
                script.parent = gameObject;
                Collider2D parentCollider = gameObject.GetComponent<Collider2D>();
                Collider2D projectileCollider = Projectile.GetComponent<Collider2D>();
                if (parentCollider != null && projectileCollider != null)
                {
                    Physics2D.IgnoreCollision(projectileCollider, parentCollider);
                }
            }

            Rigidbody2D rb = Projectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * projectileSpeed;
            Destroy(Projectile, projectileDuration);
        }
    }
}*/