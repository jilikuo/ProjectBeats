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

    public class JinxMinigun : IShootable
    {
        // CONSTANTES DE CONFIGURAÇÃO DA ARMA
        private readonly int BaseProjectiles        = 3;                            // TRÊS PROJÉTEIS
        private readonly int BaseProjectileSpeed    = 5;                            // VELOCIDADE BASE DE 5
        private readonly int TriggerSpeedFactor     = 5;                            // 20% DO COOLDOWN ( X / 5 = 0,2x) A CADA DISPARO
        private readonly int MaxProjectileDuration  = 5;                            // DURAÇÃO MÁXIMA DE 5 SEGUNDOS
        private readonly float OffsetValue          = 0.1f;                         // DISPERSÃO DE 0.1 UNIDADES
        protected readonly WeaponTypes Type         = WeaponTypes.JinxMinigun;      // TIPO DE ARMA

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
                    _range = Player.AttackRange.Value;
                    DirtyStat.Remove(Player.AttackRange);
                    ReadDirtiness();
                }
                return _range;
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
                    _projectileSpeed = Player.ProjectileSpeed.Value;
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
                    _projectileNumber = Player.ProjectileNumber.Value;
                    DirtyStat.Remove(Player.ProjectileNumber);
                    ReadDirtiness();
                }
                return _projectileNumber + BaseProjectiles;
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

            // Inicia os ouvintes para alterações em cada stat relevante
            Player.AttackDamage.OnValueChanged      += () => BecomeDirty(Player.AttackDamage);
            Player.AttacksPerSecond.OnValueChanged  += () => BecomeDirty(Player.AttacksPerSecond);
            Player.AttackRange.OnValueChanged       += () => BecomeDirty(Player.AttackRange);
            Player.ProjectileNumber.OnValueChanged  += () => BecomeDirty(Player.ProjectileNumber);
            Player.ProjectileSpeed.OnValueChanged   += () => BecomeDirty(Player.ProjectileSpeed);

            // Adiciona os stats à lista de DirtyStat
            DirtyStat.Add(Player.AttackDamage);
            DirtyStat.Add(Player.AttacksPerSecond);
            DirtyStat.Add(Player.AttackRange);
            DirtyStat.Add(Player.ProjectileNumber);
            DirtyStat.Add(Player.ProjectileSpeed);

            this.CooldownTimer = this.Cooldown;
            this.TriggerSpeed = (this.Cooldown / TriggerSpeedFactor) / this.ProjectileNumber; // A VELOCIDADE DE GATILHO É 20% DO COOLDOWN DIVIDIDO ENTRE O TOTAL DE PROJÉTEIS A SEREM DISPARADOS
            
        }

        //TODO: implementar lógica de recarregar os valores dos status relevantes conforme necessário
        // caso o jogador tenha um item que aumente o dano, por exemplo, o dano do projétil deve ser recalculado
        // caso o jogador suba de nível e aumente qualquer um dos status relevantes, as variáveis associadas a esse status
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
                Debug.Log("JINX MINIGUN NAO ESTÁ MAIS SUJO");
                isDirty = false;
            }
        }

        public float ReturnStatValueByType(StatType type)
        {
            switch (type)
            {
                case StatType.AttackDamage:
                    return Damage;
                case StatType.AttackRange:
                    return Range;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
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
                Debug.Log("AIMING PROJECTILES");
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

            //atira na direção calculada, uma vez para cada projétil disponível, considerando o tempo de gatilho
            for (int i = 0; i < ProjectileNumber; i++)
            {
                Shoot(direction);
                yield return new WaitForSeconds(TriggerSpeed);
            }
        }

        public void Shoot(Vector2 direction)
        {
            //adiciona um pequeno spread para o disparo
            float offset = OffsetValue;
            float offsetX = Random.Range(-offset, offset);
            float offsetY = Random.Range(-offset, offset);
            Vector3 spawnPosition = PlayerTransform.position + new Vector3(offsetX, offsetY, 0);


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
