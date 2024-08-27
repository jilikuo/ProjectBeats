using Jili.StatSystem.EntityTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jili.StatSystem.AttackSystem
{
    // APLICAR FÓRMULAS ESPECIFICAS DE ARMAS NO RETURN DE CADA ARMA É UMA BOA IDEIA? 
    // PARECE QUE SERIA MAIS PRÁTICO TER UMA CLASSE ESTÁTICA PARA CÁLCULO DE FÓRMULAS, 
    // TALVEZ IMPLEMENTE DESSA FORMA NO FUTURO;

    public class JinxShotgun : IShootable
    {
        // CONSTANTES DE CONFIGURAÇÃO DA ARMA
        private readonly int BaseProjectiles        = 2;                            
        private readonly int BaseProjectileSpeed    = 10;                            // VELOCIDADE BASE DE 5
        private readonly int MaxProjectileDuration  = 5;                            // DURAÇÃO MÁXIMA DE 5 SEGUNDOS
        protected readonly WeaponTypes Type         = WeaponTypes.JinxMinigun;      // TIPO DE ARMA

        //projectile damage
        private float _damage;
        protected float Damage 
        { 
            get {
                if (isDirty && DirtyStat.Contains(Player.AttackDamage))
                {
                    Debug.Log("LOADING BULLET DAMAGE");
                    _damage = Player.AttackDamage.ReadValue();
                    DirtyStat.Remove(Player.AttackDamage);
                    ReadDirtiness();
                }
                return _damage * 0.75f; // o dano do projétil é
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
                    _cooldown = Player.AttacksPerSecond.ReadValue();
                    DirtyStat.Remove(Player.AttacksPerSecond);
                    ReadDirtiness();
                }
                return 0.75f / _cooldown; // if dex = 0 && finesse = 0, attacks per second = 0.333, para fazer o jogador atacar uma vez a cada 3 segundos
                                      // a fórmula precisa ser ajustada como 1 / 0.333 ~= 3 segundos (?)
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
                    _range = Player.AttackRange.ReadValue();
                    DirtyStat.Remove(Player.AttackRange);
                    ReadDirtiness();
                }
                return _range * 1.5f;
            }
        }
        
        // not working
        // spread
        private float _spread;        // dispersão dos projéteis
        protected float Spread
        {
            get
            {
                if (isDirty)
                {
                    _spread = Player.AttackRange.ReadValue();
                    ReadDirtiness();
                }
                return Mathf.Max(15, 90 - (_spread * 1.5f));
            }
        }

        // projectile speed
        private float _projectileSpeed;
        protected float ProjectileSpeed
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileSpeed))
                {
                    _projectileSpeed = Player.ProjectileSpeed.ReadValue();
                    DirtyStat.Remove(Player.ProjectileSpeed);
                    ReadDirtiness();
                }
                return _projectileSpeed + BaseProjectileSpeed;
            }
        }

        // number of projectiles
        private float   _projectileNumber;        // número de projéteis do jogador
        protected float ProjectileNumber
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileNumber))
                {
                    _projectileNumber = Player.ProjectileNumber.ReadValue();
                    DirtyStat.Remove(Player.ProjectileNumber);
                    ReadDirtiness();
                }
                return _projectileNumber + BaseProjectiles;
            }
        }

        protected List<Stat> DirtyStat;
        protected Boolean isDirty = true;
        protected GameObject Projectile;
        protected PlayerIdentity Player;
        protected Transform PlayerTransform;

        public JinxShotgun(GameObject projectile, PlayerIdentity player)
        {
            this.Projectile = projectile;
            this.Player = player;
            this.PlayerTransform = player.transform;
            this.DirtyStat = new List<Stat>();

            // Inicia os ouvintes para alterações em cada stat relevante
            Player.AttackDamage.OnValueChanged      += BecomeDirty;
            Player.AttacksPerSecond.OnValueChanged  += BecomeDirty;
            Player.AttackRange.OnValueChanged       += BecomeDirty;
            Player.ProjectileNumber.OnValueChanged  += BecomeDirty;
            Player.ProjectileSpeed.OnValueChanged   += BecomeDirty;

            // Adiciona os stats à lista de DirtyStat
            DirtyStat.Add(Player.AttackDamage);
            DirtyStat.Add(Player.AttacksPerSecond);
            DirtyStat.Add(Player.AttackRange);
            DirtyStat.Add(Player.ProjectileNumber);
            DirtyStat.Add(Player.ProjectileSpeed);

            this.CooldownTimer = this.Cooldown;            
        }

        public JinxShotgun(PlayerIdentity player) : this(GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().JinxShotgunBullet, player) { }

        public JinxShotgun() : this(GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().JinxShotgunBullet, GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIdentity>()) { }


        public void BecomeDirty(Stat stat) 
        {
            isDirty = true;
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
        }

        public void ReadDirtiness()
        {
            if (DirtyStat.Count > 0)
            {
                isDirty = true;
            }
            else
            {
                isDirty = false;
            }
        }

        public float ReturnStatValueByType(StatType type)
        {
            float value = 0f;
            switch (type)
            {
                case StatType.AttackDamage:
                    value = Damage;
                    break;
                case StatType.AttackRange:
                    value = Range;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return value;
        }

        public Type ReadClassType()
        {
            return this.GetType();
        }

        public void IncreaseTier()
        { 
        
        }

            public bool TryShoot(float deltaTime)
        {
            //OPERA O COOLDOWN
            CooldownTimer -= deltaTime;

            //SE O COOLDOWN ESTIVER ZERADO, ATIRA
            if (CooldownTimer <= 0)
            {
                CooldownTimer = Cooldown; // ZERA O COOLDOWN NOVAMENTE
                //Debug.Log("AIMING PROJECTILES");
                Player.StartCoroutine(AimProjectiles()); // INICIA A CORROTINA DE MIRA
                return true;
            }
            return false;
        }

        public IEnumerator AimProjectiles()
        {
            //calcula onde o mouse está mirando
            Vector3 shootPos = PlayerTransform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - shootPos).normalized;

            float angleStep = Spread / ProjectileNumber;
            float currentAngle = -angleStep * (ProjectileNumber - 1) / 2; // Centraliza os projéteis

            //atira na direção calculada
            for (int i = 0; i < ProjectileNumber; i++)
            {
                // Calcula a nova direção com base no ângulo de rotação
                Vector2 rotatedDirection = RotateVector(direction, currentAngle);

                // Atira na direção calculada
                Shoot(rotatedDirection);

                currentAngle += angleStep;
            }

            yield return null;
        }

        private Vector2 RotateVector(Vector2 originalVector, float angleDegrees)
        {
            float radians = angleDegrees * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            float x = originalVector.x * cos - originalVector.y * sin;
            float y = originalVector.x * sin + originalVector.y * cos;
            return new Vector2(x, y).normalized;
        }

        public void Shoot(Vector2 direction)
        {
            Vector3 spawnPosition = PlayerTransform.position;


            //instancia e configura o projétil
            GameObject NewProjectile = UnityEngine.Object.Instantiate(Projectile, spawnPosition, Quaternion.identity);
            ProjectileBase projectile = NewProjectile.GetComponent<ProjectileBase>();
            if (projectile != null)
            {
                projectile.Parent = Player.gameObject;
                projectile.source = this;
                Collider2D parentCollider = projectile.Parent.GetComponent<Collider2D>();
                Collider2D projectileCollider = NewProjectile.GetComponent<Collider2D>();
                if (parentCollider != null && projectileCollider != null)
                {
                    Physics2D.IgnoreCollision(projectileCollider, parentCollider);
                }
            }

            //aplica a velocidade ao projétil   
            Rigidbody2D rb = NewProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * ProjectileSpeed;
        
            //destrói o projétil após 5 segundos
            UnityEngine.Object.Destroy(NewProjectile, MaxProjectileDuration);
        }
    }
}
