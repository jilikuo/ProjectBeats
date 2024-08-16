using Jili.StatSystem.EntityTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jili.StatSystem.AttackSystem
{
    // APLICAR F�RMULAS ESPECIFICAS DE ARMAS NO RETURN DE CADA ARMA � UMA BOA IDEIA? 
    // PARECE QUE SERIA MAIS PR�TICO TER UMA CLASSE EST�TICA PARA C�LCULO DE F�RMULAS, 
    // TALVEZ IMPLEMENTE DESSA FORMA NO FUTURO;

    public class JinxMinigun : IShootable
    {
        protected WeaponTypes Type = WeaponTypes.JinxMinigun;

        //projectile damage
        private float _damage;
        protected float Damage 
        { 
            get {
                if (isDirty && DirtyStat.Contains(Player.AttackDamage))
                {
                    _damage = Player.AttackDamage.Value;
                    DirtyStat.Remove(Player.AttackDamage);
                    ReadDirtiness();
                }
                return _damage;
            }
        }

        //attack cooldown
        private float _cooldown;
        protected float Cooldown 
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.AttacksPerSecond))
                {
                    _cooldown = Player.AttacksPerSecond.Value;
                    DirtyStat.Remove(Player.AttacksPerSecond);
                    ReadDirtiness();
                }
                return 1 / _cooldown; // if dex = 0 && finesse = 0, attacks per second = 0.333, para fazer o jogador atacar uma vez a cada 3 segundos
                                      // a f�rmula precisa ser ajustada como 1 / 0.333 ~= 3 segundos (?)
            }
        }
        protected float CooldownTimer;

        // projectile range
        private float _range;
        protected float Range
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.AttackRange))
                {
                    _range = Player.AttackRange.Value;
                    DirtyStat.Remove(Player.AttackRange);
                    ReadDirtiness();
                }
                return _range;
            }
        }

        // projectile speed
        protected float baseProjectileSpeed = 5; // velocidade padr�o do proj�til
        private float _projectileSpeed;
        protected float ProjectileSpeed
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileSpeed))
                {
                    _projectileSpeed = Player.ProjectileSpeed.Value;
                    DirtyStat.Remove(Player.ProjectileSpeed);
                    ReadDirtiness();
                }
                return _projectileSpeed + baseProjectileSpeed;
            }
        }

        // number of projectiles
        protected float baseProjectileNumber = 3; // n�mero de proj�teis padr�o da arma
        private float   _projectileNumber;        // n�mero de proj�teis do jogador
        protected float ProjectileNumber
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileNumber))
                {
                    _projectileNumber = Player.ProjectileNumber.Value;
                    DirtyStat.Remove(Player.ProjectileNumber);
                    ReadDirtiness();
                }
                return _projectileNumber + baseProjectileNumber;
            }
        }

        protected float TriggerSpeed { get; set; }
        protected List<Stat> DirtyStat;
        protected Boolean isDirty = true;
        protected GameObject Projectile;
        protected PlayerIdentity Player;
        protected Transform PlayerTransform;

        public JinxMinigun(GameObject projectile, GameObject player)
        {
            this.Projectile = projectile;
            this.Player = player.GetComponent<PlayerIdentity>();
            this.PlayerTransform = player.transform;
            this.DirtyStat = new List<Stat>();

            // Inicia os ouvintes para altera��es em cada stat relevante
            Player.AttackDamage.OnValueChanged      += () => BecomeDirty(Player.AttackDamage);
            Player.AttacksPerSecond.OnValueChanged  += () => BecomeDirty(Player.AttacksPerSecond);
            Player.AttackRange.OnValueChanged       += () => BecomeDirty(Player.AttackRange);
            Player.ProjectileNumber.OnValueChanged  += () => BecomeDirty(Player.ProjectileNumber);
            Player.ProjectileSpeed.OnValueChanged   += () => BecomeDirty(Player.ProjectileSpeed);

            // Adiciona os stats � lista de DirtyStat
            DirtyStat.Add(Player.AttackDamage);
            DirtyStat.Add(Player.AttacksPerSecond);
            DirtyStat.Add(Player.AttackRange);
            DirtyStat.Add(Player.ProjectileNumber);
            DirtyStat.Add(Player.ProjectileSpeed);

            this.CooldownTimer = this.Cooldown;
            this.TriggerSpeed = (this.Cooldown / 5) / this.ProjectileNumber; // A VELOCIDADE DE GATILHO � 20% DO COOLDOWN DIVIDIDO ENTRE O TOTAL DE PROJ�TEIS A SEREM DISPARADOS
            
        }

        //TODO: implementar l�gica de recarregar os valores dos status relevantes conforme necess�rio
        // caso o jogador tenha um item que aumente o dano, por exemplo, o dano do proj�til deve ser recalculado
        // caso o jogador suba de n�vel e aumente qualquer um dos status relevantes, as vari�veis associadas a esse status
        // devem ser recalculadas
        public void BecomeDirty(Stat stat) 
        {
            if (DirtyStat != null )
            {
                if (!DirtyStat.Contains(stat))
                {
                    DirtyStat.Add(stat);
                }
            }
            else
            {
                DirtyStat = new List<Stat> { stat };
            }

            isDirty = true;
        }

        public void ReadDirtiness()
        {
            if (DirtyStat.Count > 0)
            {
                isDirty = true;
            }
            else
            {
                Debug.Log("JINX MINIGUN NAO EST� MAIS SUJO");
                isDirty = false;
            }
        }


        public bool TryShoot(float deltaTime)
        {
            //OPERA O COOLDOWN
            CooldownTimer -= deltaTime;

            //SE O COOLDOWN ESTIVER ZERADO, ATIRA
            if (CooldownTimer <= 0)
            {
                CooldownTimer = Cooldown; // ZERA O COOLDOWN NOVAMENTE
                Debug.Log("Projectile Number: " + ProjectileNumber + "being base = " + baseProjectileNumber + "and from player = " + _projectileNumber);
                Debug.Log("AIMING PROJECTILES");
                Player.StartCoroutine(AimProjectiles()); // INICIA A CORROTINA DE MIRA
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
            rb.velocity = direction.normalized * ProjectileSpeed;
        
            //destr�i o proj�til ap�s 5 segundos
            UnityEngine.Object.Destroy(NewProjectile, 5);
        }
    }
}
