using Jili.StatSystem.EntityTree;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jili.StatSystem.AttackSystem
{
    public class JinxMinigun : IShootable
    {
        protected WeaponTypes Type = WeaponTypes.JinxMinigun;
        protected float Damage;
        protected float Cooldown;
        protected float CooldownTimer;
        protected float Range;
        protected float Speed;
        protected float ProjectileNumber;
        protected float TriggerSpeed;
        protected Boolean isDirty = true;
        protected GameObject Projectile;
        protected PlayerIdentity Player;
        protected Transform PlayerTransform;

        public JinxMinigun(float damage, float cooldown, float range, float speed, float projectilenumber, GameObject projectile, GameObject player)
        {
            this.Damage = damage;
            this.Cooldown = cooldown;
            this.CooldownTimer = cooldown;
            this.Range = range;
            this.Speed = speed;
            this.ProjectileNumber = projectilenumber;
            this.TriggerSpeed = (cooldown / 5) / projectilenumber; // A VELOCIDADE DE GATILHO � 20% DO COOLDOWN DIVIDIDO ENTRE O TOTAL DE PROJ�TEIS A SEREM DISPARADOS
            this.Projectile = projectile;
            this.Player = player.GetComponent<PlayerIdentity>();
            this.PlayerTransform = player.transform;
        }

        //TODO: implementar l�gica de recarregar os valores dos status relevantes conforme necess�rio
        // caso o jogador tenha um item que aumente o dano, por exemplo, o dano do proj�til deve ser recalculado
        // caso o jogador suba de n�vel e aumente qualquer um dos status relevantes, as vari�veis associadas a esse status
        // devem ser recalculadas
        public void BecomeDirty()
        {
            isDirty = true;
        }

        public void CleanUp()
        {
            isDirty = false;
        }

        public bool TryShoot(float deltaTime)
        {
            //OPERA O COOLDOWN
            CooldownTimer -= deltaTime;

            //SE O COOLDOWN ESTIVER ZERADO, ATIRA
            if (CooldownTimer <= 0)
            {
                Player.StartCoroutine(AimProjectiles()); // INICIA A CORROTINA DE MIRA
                CooldownTimer = Cooldown; // ZERA O COOLDOWN NOVAMENTE
                return true;
            }
            return false;
        }

        public IEnumerator AimProjectiles()
        {
            //calcula onde o mouse est� mirando
            Vector3 shootPos = PlayerTransform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - shootPos).normalized;

            //atira na dire��o calculada, uma vez para cada proj�til dispon�vel, considerando o tempo de gatilho (20% do cooldown, dividido entre o total de proj�teis)
            for (int i = 0; i < ProjectileNumber; i++)
            {
                Shoot(direction);
                yield return new WaitForSeconds(TriggerSpeed);
            }
        }

        public void Shoot(Vector2 direction)
        {
            //adiciona um pequeno spread para o disparo
            float offset = 0.05f;
            float offsetX = Random.Range(-offset, offset);
            float offsetY = Random.Range(-offset, offset);
            Vector3 spawnPosition = PlayerTransform.position + new Vector3(offsetX, offsetY, 0);


            //instancia e configura o proj�til
            GameObject NewProjectile = UnityEngine.Object.Instantiate(Projectile, spawnPosition, Quaternion.identity);
            Projectile script = NewProjectile.GetComponent<Projectile>();
            if (script != null)
            {
                script.parent = Player.gameObject;
                script.range = Range;
                Collider2D parentCollider = script.parent.GetComponent<Collider2D>();
                Collider2D projectileCollider = NewProjectile.GetComponent<Collider2D>();
                if (parentCollider != null && projectileCollider != null)
                {
                    Physics2D.IgnoreCollision(projectileCollider, parentCollider);
                }
            }

            //aplica a velocidade ao proj�til   
            Rigidbody2D rb = NewProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction * Speed;
        
            //destr�i o proj�til ap�s 5 segundos
            UnityEngine.Object.Destroy(NewProjectile, 5);
        }
    }
}
